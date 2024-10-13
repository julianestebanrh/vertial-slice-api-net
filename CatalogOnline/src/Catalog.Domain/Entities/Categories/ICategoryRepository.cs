namespace Catalog.Domain.Entities.Categories
{
	public interface ICategoryRepository
	{
		Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
		Task<List<Category>> GetAllAsync(CancellationToken cancellationToken);
		void Add(Category category);
	}
}
