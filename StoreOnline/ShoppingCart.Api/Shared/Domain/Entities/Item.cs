namespace ShoppingCart.Api.Shared.Domain.Entities
{
	public sealed class Item : Entity
	{

		private Item(
			Guid id,
			string code,
			string image,
			int quantity,
			decimal price,
			string name,
			string description,
			Guid cartId) : base(id)
		{
			Id = id;
			Code = code;
			Image = image;
			Quantity = quantity;
			Price = price;
			Name = name;
			Description = description;
			CartId = cartId;
		}

		private Item() { }

		public string? Code { get; private set; }
		public string? Image { get; private set; }
		public decimal Price { get; private set; }
		public int Quantity { get; private set; }
		public string? Name { get; private set; }
		public string? Description { get; private set; }
		public Guid CartId { get; private set; }
		public Cart? Cart { get; private set; }

		public static Item Create(string code, string image, int quantity, decimal price, string name, string description)
		{
			return new Item(Guid.NewGuid(), code, image, quantity, price, name, description, default);
		}

		public static Item Create(string code, string image, int quantity, decimal price, string name, string description, Guid cartId)
		{
			return new Item(Guid.NewGuid(), code, image, quantity, price, name, description, cartId);
		}

		public void Update(string code, string image, int quantity, decimal price, string name, string description)
		{
			Code = code;
			Image = image;
			Quantity = quantity;
			Price = price;
			Name = name;
			Description = description;
		}
	}
}
