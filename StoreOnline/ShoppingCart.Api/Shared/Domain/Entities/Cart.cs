namespace ShoppingCart.Api.Shared.Domain.Entities
{
	//public sealed class Cart(
	//	Guid id,
	//	string code,
	//	string userId) : Entity(id)
	//{
	//	private Cart() : this(Guid.Empty, string.Empty, string.Empty) { }

	//	public string Code { get; private set; } = code;
	//	public string UserId { get; private set; } = userId;
	//	public ICollection<Item> Items { get; set; } = [];

	//	public static Cart Create(string code, string userId)
	//	{
	//		var cart = new Cart(Guid.NewGuid(), code, userId);
	//		cart.UserId = userId;
	//		cart.CreatedOn = DateTime.UtcNow;
	//		return cart;
	//	}
	//}

	public sealed class Cart : Entity
	{
		private Cart() { }
		private Cart(Guid id, string code, string userId) : base(id)
		{
			Code = code;
			UserId = userId;
		}

		public string Code { get; private set; }
		public string UserId { get; private set; }
		public ICollection<Item> Items { get; set; } = [];

	}
}
