using Catalog.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
	internal abstract class Repository<T> where T : Entity
	{
		protected readonly CatalogDbContext DbContext;

		protected Repository(CatalogDbContext dbContext)
		{
			DbContext = dbContext;
		}

		public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			return await DbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
		}

		public void Add(T entity)
		{
			DbContext.Add(entity);
		}
	}
}
