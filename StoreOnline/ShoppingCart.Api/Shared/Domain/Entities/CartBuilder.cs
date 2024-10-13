using System.Globalization;
using System.Reflection;

namespace ShoppingCart.Api.Shared.Domain.Entities
{
	public class CartBuilder
	{
		private DateTime? _createdOn;
		private string? _userId;
		private string? _code;
		private ICollection<Item> _items = new List<Item>();

		public static CartBuilder Create()
		{
			return new CartBuilder();
		}

		public CartBuilder SetCreatedOn(DateTime? createdOn)
		{
			_createdOn = createdOn;
			return this;
		}

		public CartBuilder SetUserId(string userId)
		{
			_userId = userId;
			return this;
		}

		public CartBuilder SetCode(string code)
		{
			_code = code;
			return this;
		}

		public CartBuilder AddItem(Item item)
		{
			_items.Add(item);
			return this;
		}

		public Cart Build(Guid id)
		{
			var cart = (Cart)Activator.CreateInstance(
				typeof(Cart),
				BindingFlags.NonPublic | BindingFlags.Instance,
				default(Binder),
				[id, _code!, _userId!],
				default(CultureInfo)!
			)!;

			if (cart.CreatedOn is null)
			{
				cart.CreatedOn = DateTime.UtcNow;
			}

			if (string.IsNullOrEmpty(cart.CreatedBy))
			{
				cart.CreatedBy = _userId!;
			}

			if (_items.Any())
			{
				cart.Items = _items;
			}

			return cart;
		}
	}
}
