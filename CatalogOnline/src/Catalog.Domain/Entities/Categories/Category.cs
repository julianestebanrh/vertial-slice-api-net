using Catalog.Domain.Abstractions;
using Catalog.Domain.Entities.Categories.Events;

namespace Catalog.Domain.Entities.Categories
{
	public class Category : Entity
	{
		private Category() { }
		private Category(Guid id, string name) : base(id)
		{
			Name = name;
		}

		public string Name { get; private set; }

		public static Category Create(string name)
		{
			var category = new Category(Guid.NewGuid(), name);
			category.RaiseDomainEvent(new CategoryCreatedDomainEvent(category.Id));
			return category;
		}
	}
}
