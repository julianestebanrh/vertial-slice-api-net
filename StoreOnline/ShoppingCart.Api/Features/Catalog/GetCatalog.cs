using ShoppingCart.Api.Shared.Networking.Catalog.Api;
using ShoppingCart.Api.Shared.Slices;

namespace ShoppingCart.Api.Features.Catalog
{
	public class GetCatalog : IEndpoint
	{
		public void AddEndpoint(IEndpointRouteBuilder app)
		{
			app.MapGet("api/catalog", async (ILoggerFactory loggerFactory, ICatalogApiClient catalogApiClient, CancellationToken cancellationTolen) =>
			{
				loggerFactory.CreateLogger("EndpointCatalog_Get").LogInformation("Products Catalog");

				var result = await catalogApiClient.GetProductsAsync(cancellationTolen);
				return Results.Ok(result);
			});
		}
	}
}
