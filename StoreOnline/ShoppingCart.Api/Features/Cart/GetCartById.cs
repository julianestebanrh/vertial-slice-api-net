using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Api.Shared.Persistence;
using ShoppingCart.Api.Shared.Slices;

namespace ShoppingCart.Api.Features.Cart
{
	public class GetCartById : IEndpoint
	{
		public void AddEndpoint(IEndpointRouteBuilder app)
		{
			app.MapGet("api/carts/{id}", async (Guid id, IMediator mediator, CancellationToken cancellationTolen) =>
			{
				var query = new GetCartByIdQuery(id);
				return await mediator.Send(query, cancellationTolen);
			});
		}

		public sealed class GetCartByIdQueryHandler(ShoppingDbContext context, IMapper mapper) : IRequestHandler<GetCartByIdQuery, IResult>
		{
			private readonly IMapper _mapper = mapper;
			private readonly ShoppingDbContext _context = context;

			public async Task<IResult> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
			{
				var carts = await _context.Carts.Include(x => x.Items).Where(x => x.Id == request.Id).FirstOrDefaultAsync();

				if (carts is null)
				{
					return Results.NotFound();
				}

				var result = _mapper.Map<CartDto>(carts);
				return Results.Ok(result);
			}
		}

		public sealed class GetCartByIdQuery(Guid id) : IRequest<IResult>
		{
			public Guid Id { get; } = id;
		}

		public sealed class GetCartByIdResponse(CartDto cart)
		{
			public CartDto Cart { get; } = cart;
		}

		public sealed class CartMapProfile : Profile
		{
			public CartMapProfile()
			{
				CreateMap<Shared.Domain.Entities.Cart, CartDto>();
				CreateMap<Shared.Domain.Entities.Item, ItemDto>();
			}
		}

		public sealed class CartDto
		{
			public required Guid Id { get; set; }
			public required string Code { get; set; }
			public string? UserId { get; set; }
			public List<ItemDto> Items { get; set; } = [];
		}

		public sealed class ItemDto
		{
			public required Guid Id { get; set; }
			public required string Code { get; set; }
			public string? Image { get; set; }
			public required decimal Price { get; set; }
			public required int Quantity { get; set; }
			public required string Name { get; set; }
			public string? Description { get; set; }

		}
	}
}
