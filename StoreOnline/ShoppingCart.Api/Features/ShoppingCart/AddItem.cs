using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingCart.Api.Shared.Domain.Entities;
using ShoppingCart.Api.Shared.Networking.Catalog.Api;
using ShoppingCart.Api.Shared.Persistence;
using ShoppingCart.Api.Shared.Slices;

namespace ShoppingCart.Api.Features.ShoppingCart
{
	public sealed class AddItem : IEndpoint
	{
		public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
		{
			endpointRouteBuilder.MapPost("api/carts/{cartId}/items", async (Guid cartId, AddItemRequest request, IMediator mediator, CancellationToken cancellationToken) =>
			{
				var command = new AddItemCommand(cartId, request.Code, request.Quantity);
				return await mediator.Send(command, cancellationToken);
			});
		}

		public sealed class AddItemRequest(string code, int quantity)
		{
			public string Code { get; set; } = code;
			public int Quantity { get; set; } = quantity;
		}

		public sealed class AddItemCommand(Guid cartId, string code, int quantity) : IRequest<IResult>
		{
			public Guid CartId { get; set; } = cartId;
			public string Code { get; set; } = code;
			public int Quantity { get; set; } = quantity;
		}

		public sealed class AddItemCommandValidator : AbstractValidator<AddItemCommand>
		{
			public AddItemCommandValidator()
			{

				RuleFor(x => x.Code).NotEmpty().NotNull().MaximumLength(200);
				RuleFor(x => x.Quantity).NotNull().NotEmpty().GreaterThanOrEqualTo(1);
			}
		}

		public sealed class AddItemCommandHandler(ICatalogApiClient catalogApiClient, ShoppingDbContext context, IMapper mapper) : IRequestHandler<AddItemCommand, IResult>
		{
			private readonly IMapper _mapper = mapper;
			private readonly ShoppingDbContext _context = context;
			private readonly ICatalogApiClient _catalogApiClient = catalogApiClient;

			public async Task<IResult> Handle(AddItemCommand request, CancellationToken cancellationToken)
			{
				// 1. Buscar el producto del catalog por codigo
				var product = await _catalogApiClient.GetProductByCodeAsync(request.Code, cancellationToken);

				if (product is null)
				{
					return Results.Problem(
						title: "Product Not Found",
						statusCode: StatusCodes.Status400BadRequest,
						detail: $"The product code not exist in catalog: {request.Code}");
				}

				// 2. Crear el objeto de tipo item basado en la clase entidad Item
				var item = Item.Create(product.Code, product.Image, request.Quantity, product.Price ?? product.Price!.Value, product.Name, product.Description, request.CartId);

				// 3. Insertar el producto en la base de datos
				_context.Items.Add(item);
				await _context.SaveChangesAsync(cancellationToken);

				var itemDto = _mapper.Map<ItemDto>(item);

				return Results.Created($"api/items/{itemDto.Id}", itemDto);
			}
		}

		public sealed class ItemDto
		{
			public required Guid Id { get; set; }
			public required string Code { get; set; }
			public required string Image { get; set; }
			public required decimal Price { get; set; }
			public required int Quantity { get; set; }
			public required string Description { get; set; }
			public required Guid CartId { get; set; }
		}

		public sealed class ItemMapProfile : Profile
		{
			public ItemMapProfile()
			{
				CreateMap<Item, ItemDto>();
			}
		}
	}
}
