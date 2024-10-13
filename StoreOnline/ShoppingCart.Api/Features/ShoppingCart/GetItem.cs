using AutoMapper;
using MediatR;
using ShoppingCart.Api.Shared.Domain.Entities;
using ShoppingCart.Api.Shared.Persistence;
using ShoppingCart.Api.Shared.Slices;

namespace ShoppingCart.Api.Features.ShoppingCart
{
	public class GetItem : IEndpoint
	{
		public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
		{
			endpointRouteBuilder.MapGet("api/items/{id}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
			{
				var query = new GetItemQuery(id);
				return await mediator.Send(query, cancellationToken);
			}).WithName("GetItem");
		}

		public sealed class GetItemQuery(Guid id) : IRequest<IResult>
		{
			public Guid Id { get; } = id;
		}

		public sealed class GetItemQueryHandler(ShoppingDbContext context, IMapper mapper) : IRequestHandler<GetItemQuery, IResult>
		{
			private readonly IMapper _mapper = mapper;
			private readonly ShoppingDbContext _context = context;

			public async Task<IResult> Handle(GetItemQuery request, CancellationToken cancellationToken)
			{
				var item = await _context.Items.FindAsync(request.Id, cancellationToken);

				if (item == null)
				{
					return Results.NotFound();
				}

				var itemDto = _mapper.Map<ItemDto>(item);

				return Results.Ok(itemDto);

			}

			public sealed class ItemDto
			{
				public required Guid Id { get; set; }
				public required string Name { get; set; }
				public required string Description { get; set; }
				public required string Image { get; set; }
				public required decimal Price { get; set; }
				public required decimal PricePrice { get; set; }

			}

			public sealed class GetItemMapProfile : Profile
			{
				public GetItemMapProfile()
				{
					CreateMap<Item, ItemDto>();
				}
			}
		}
	}
}
