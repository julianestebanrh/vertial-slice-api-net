namespace ShoppingCart.Api.Shared.Domain.Models
{
	public class Catalog
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public string? Code { get; set; }
		public string? Description { get; set; }
		public string? Image { get; set; }
		public decimal? Price { get; set; }
		public Guid CategoryId { get; set; }
	}
}
