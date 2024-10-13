using Dapper;
using MediatR;
using ShoppingCart.Api.Shared.Data;
using ShoppingCart.Api.Shared.Slices;

namespace ShoppingCart.Api.Features.Cart
{
	public class GetCartByCode : IEndpoint
	{
		public void AddEndpoint(IEndpointRouteBuilder app)
		{
			app.MapGet("api/carts/{code}", async (string code, IMediator mediator, CancellationToken cancellationTolen) =>
			{
				var query = new GetCartByCodeQuery(code);
				return await mediator.Send(query, cancellationTolen);

			});

		}

		public sealed class GetCartByCodeQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IRequestHandler<GetCartByCodeQuery, IResult>
		{
			private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

			public async Task<IResult> Handle(GetCartByCodeQuery request, CancellationToken cancellationToken)
			{
				const string sql = """
					SELECT 
						a.id as Id,
						a.code as Code,
						a.user_id as UserId,
						b.id as ItemId,
						b.name as Name,
						b.description as Description
					FROM shopping_carts a
						LEFT JOIN items b ON a.id = b.cart_id
					WHERE a.code = @Code
				""";

				var cartDictionary = new Dictionary<string, CartDto>();

				using var connection = _sqlConnectionFactory.CreateConnection();
				await connection.QueryAsync<CartDto, ItemDto, CartDto>(sql, (cart, item) =>
				{
					if (cartDictionary.TryGetValue(cart.Code!, out var existingCart))
					{
						cart = existingCart;
					}
					else
					{
						cartDictionary.Add(cart.Code!, cart);
					}
					cart.Items.Add(item);

					return cart;
				}, new { Code = request.Code }, splitOn: "ItemId");

				if (cartDictionary.Count == 0)
				{
					return Results.NotFound();
				}

				var result = cartDictionary.Values.ToList();
				return Results.Ok(new GetCartByCodeResponse(result));
			}
		}

		public sealed class GetCartByCodeQuery(string code) : IRequest<IResult>
		{
			public string Code { get; } = code;
		}

		public sealed class GetCartByCodeResponse(IEnumerable<CartDto> carts)
		{
			public IEnumerable<CartDto> Carts { get; } = carts;
		}

		public sealed class CartDto
		{
			public required Guid Id { get; set; }
			public required string Code { get; set; }
			public required string UserId { get; set; }
			public List<ItemDto> Items { get; init; } = [];
		}

		public sealed class ItemDto
		{
			public required Guid ItemId { get; set; }
			public required string Code { get; set; }
			public string? Description { get; set; }

		}
	}
}
