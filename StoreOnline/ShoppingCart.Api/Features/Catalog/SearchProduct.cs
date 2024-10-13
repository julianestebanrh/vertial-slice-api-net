using ShoppingCart.Api.Shared.Networking.Catalog.Api;
using ShoppingCart.Api.Shared.Slices;

namespace ShoppingCart.Api.Features.Catalog
{
	public class SearchProduct : IEndpoint
	{
		public void AddEndpoint(IEndpointRouteBuilder app)
		{
			app.MapGet("api/catalog/code/{code}", async (string code, ILoggerFactory loggerFactory, ICatalogApiClient catalogApiClient, CancellationToken cancellationTolen) =>
			{
				loggerFactory.CreateLogger("EndpointCatalog_Code").LogInformation("Search product by code");

				var result = await catalogApiClient.GetProductByCodeAsync(code, cancellationTolen);
				return Results.Ok(result);
			});
		}
	}
}
