using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Api.Shared.Domain.Entities;
using ShoppingCart.Api.Shared.Networking.Catalog.Api;
using ShoppingCart.Api.Shared.Persistence;
using ShoppingCart.Api.Shared.Slices;

namespace ShoppingCart.Api.Features.ShoppingCart
{
	public class UpdateItem : IEndpoint
	{
		public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
		{
			// Cuando es un parametro query se le coloca [FromQuery(Name = "id")] Guid id
			endpointRouteBuilder.MapPut("api/items/{id}", async ([FromRoute] Guid id, [FromBody] UpdateItemRequest request, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
			{
				var command = new UpdateItemCommand(id, request.Code, request.Quantity);
				return await mediator.Send(command, cancellationToken);
			});
		}

		public sealed class UpdateItemRequest(string code, int quantity)
		{
			public string Code { get; } = code;
			public int Quantity { get; } = quantity;
		}

		public sealed class UpdateItemCommand(Guid id, string code, int quantity) : IRequest<IResult>
		{
			public Guid Id { get; set; } = id;
			public string Code { get; set; } = code;
			public int Quantity { get; set; } = quantity;
		}

		public sealed class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
		{
			public UpdateItemCommandValidator()
			{
				RuleFor(x => x.Id).NotEmpty();
				RuleFor(x => x.Quantity).NotEmpty();
				RuleFor(x => x.Code).NotEmpty();
			}
		}

		public sealed class UpdateItemCommandHandler(ShoppingDbContext context, IMapper mapper, ICatalogApiClient catalogApiClient) : IRequestHandler<UpdateItemCommand, IResult>
		{
			private readonly IMapper _mapper = mapper;
			private readonly ShoppingDbContext _context = context;
			private readonly ICatalogApiClient _catalogApiClient = catalogApiClient;

			public async Task<IResult> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
			{
				var product = await _catalogApiClient.GetProductByCodeAsync(request.Code, cancellationToken);
				if (product is null)
				{
					return Results.NotFound();
				}

				//var rowsAffected = await _context.Items
				//	.Where(x => x.Id == request.Id)
				//	.ExecuteUpdateAsync(setter => setter
				//		.SetProperty(x => x.Code, product.Code)
				//		.SetProperty(x => x.Name, product.Name)
				//		.SetProperty(x => x.Description, product.Description)
				//		.SetProperty(x => x.Price, product.Price)
				//		.SetProperty(x => x.Image, product.Image)
				//		.SetProperty(x => x.Quantity, request.Quantity));

				var item = await _context.Items.FindAsync(request.Id, cancellationToken);

				if (item is null)
				{
					return Results.NotFound();
				}

				_context.Items.Attach(item);
				item.Update(product.Code!, product.Image!, request.Quantity, product.Price ?? product.Price!.Value, product.Name!, product.Description!);
				var rowsAffected = await _context.SaveChangesAsync(cancellationToken);

				if (rowsAffected == 0)
				{
					return Results.InternalServerError();
				}

				var itemDto = _mapper.Map<ItemDto>(item);

				return Results.AcceptedAtRoute(
					routeName: "GetItem",
					routeValues: new { id = request.Id },
					itemDto
					);
			}

			public sealed class ItemDto
			{
				public required Guid Id { get; set; }
				public required string Code { get; set; }
				public required string Name { get; set; }
				public required string Description { get; set; }
				public required decimal Price { get; set; }
			}

			public sealed class ItemUpdateProfile : Profile
			{
				public ItemUpdateProfile()
				{
					CreateMap<Item, ItemDto>();
				}
			}
		}

	}
}