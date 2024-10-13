
namespace ShoppingCart.Api.Shared.Networking.Catalog.Api
{
	public interface ICatalogApiClient
	{
		Task<IEnumerable<Domain.Models.Catalog>> GetProductsAsync(CancellationToken cancellationToken);
		Task<Domain.Models.Catalog?> GetProductByCodeAsync(string code, CancellationToken cancellationToken);
	}
}
