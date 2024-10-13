namespace Catalog.Domain.Entities.Products
{
	public interface IProductRepository
	{
		Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
		Task<Product?> GetByCodeAsync(string code, CancellationToken cancellationToken);
		Task<List<Product>> GetAllAsync(CancellationToken cancellationToken);
		void Add(Product product);
	}
}
