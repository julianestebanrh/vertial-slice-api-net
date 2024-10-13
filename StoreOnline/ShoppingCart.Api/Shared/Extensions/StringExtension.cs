namespace ShoppingCart.Api.Shared.Extensions
{
	public static class StringExtension
	{
		public static string EncodedForLike(this string search)
		{
			return search.Replace("[", "[]]").Replace("%", "[%]");
		}
	}
}
