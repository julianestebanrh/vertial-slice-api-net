using Catalog.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
	internal sealed class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		public CategoryRepository(CatalogDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await (from c in DbContext.Set<Category>() select c
						  ).ToListAsync(cancellationToken);
		}
	}
}
