using Catalog.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
	internal sealed class ProductRepository : Repository<Product>, IProductRepository
	{
		public ProductRepository(CatalogDbContext dbContext) : base(dbContext)
		{
		}
		public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await DbContext.Set<Product>().ToListAsync(cancellationToken);
		}

		public async Task<Product?> GetByCodeAsync(string code, CancellationToken cancellationToken)
		{
			return await DbContext.Set<Product>().Where(x => x.Code == code).FirstOrDefaultAsync(cancellationToken);
		}
	}
}
