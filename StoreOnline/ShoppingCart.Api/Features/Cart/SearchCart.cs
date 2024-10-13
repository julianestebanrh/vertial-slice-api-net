using Dapper;
using MediatR;
using ShoppingCart.Api.Shared.Data;
using ShoppingCart.Api.Shared.Extensions;
using ShoppingCart.Api.Shared.Slices;
using System.Text;

namespace ShoppingCart.Api.Features.Cart
{
	public class SearchCart : IEndpoint
	{
		public void AddEndpoint(IEndpointRouteBuilder app)
		{
			app.MapGet("api/carts/{code}/items", async (string code, IMediator mediator, CancellationToken cancellationTolen) =>
			{
				var query = new SearchCartQuery(code);
				return await mediator.Send(query, cancellationTolen);
			});
		}

		public sealed class SearchCartQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IRequestHandler<SearchCartQuery, IResult>
		{
			private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

			public async Task<IResult> Handle(SearchCartQuery request, CancellationToken cancellationToken)
			{
				var search = "%" + request.Code.EncodedForLike() + "%";
				var whereStatement = $" WHERE a.code LIKE @Search";

				var sql = new StringBuilder("""
					SELECT
						a.id as Id,
						a.code as Code,
						b.id as ItemId,
						b.name as Name
					FROM shopping_carts a
						LEFT JOIN items b ON a.id = b.cart_id
					""");

				sql.AppendLine(whereStatement);

				var cartDictionary = new Dictionary<string, CartDto>();

				var connection = _sqlConnectionFactory.CreateConnection();

				await connection.QueryAsync<CartDto, ItemDto, CartDto>(sql.ToString(), (cart, item) =>
				{
					if (cartDictionary.TryGetValue(cart.Code, out var existingCart))
					{
						cart = existingCart;
					}
					else
					{
						cartDictionary.Add(cart.Code, cart);
					}

					cart.Items.Add(item);

					return cart;
				}, new { Code = request.Code, Search = search }, splitOn: "ItemId");

				var response = cartDictionary.Values.ToList();

				if (!response.Any())
				{
					return Results.NotFound();
				}
				return Results.Ok(response);
			}
		}

		public sealed class SearchCartQuery(string code) : IRequest<IResult>
		{
			public string Code { get; } = code;
		}

		public sealed class SearchCartResponse(IEnumerable<CartDto> carts)
		{
			public IEnumerable<CartDto> Carts { get; } = carts;
		}

		public sealed class CartDto
		{
			public required Guid Id { get; set; }
			public required string Code { get; set; }
			public List<ItemDto> Items { get; set; } = [];
		}

		public sealed class ItemDto
		{
			public required Guid ItemId { get; set; }
			public required string Name { get; set; }

		}
	}
}
