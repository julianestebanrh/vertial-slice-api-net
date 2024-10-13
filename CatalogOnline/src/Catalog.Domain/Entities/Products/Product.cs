using Catalog.Domain.Abstractions;
using Catalog.Domain.Entities.Products.Events;
using System.Text.RegularExpressions;

namespace Catalog.Domain.Entities.Products
{
	public class Product : Entity
	{
		private Product() { }
		private Product(Guid id, string name, decimal price, string description, string image, string? code, Guid categoryId) : base(id)
		{
			Name = name;
			Price = price;
			Description = description;
			Image = image;
			Code = code;
			CategoryId = categoryId;
		}

		public string? Name { get; private set; }
		public decimal? Price { get; private set; }
		public string? Description { get; private set; }
		public string? Image { get; private set; }
		public string? Code { get; set; }

		public Guid CategoryId { get; set; }

		public static Product Create(string name, decimal price, string description, string image, string code, Guid categoryId)
		{

			var id = Guid.NewGuid();

			if (string.IsNullOrWhiteSpace(code))
			{
				code = Regex.Replace(Convert.ToBase64String(id.ToByteArray()), "[/+=]", "");
			}

			var product = new Product(id, name, price, description, image, code, categoryId);
			product.RaiseDomainEvent(new ProductCreatedDomainEvent(product.Id));
			return product;
		}

	}
}
