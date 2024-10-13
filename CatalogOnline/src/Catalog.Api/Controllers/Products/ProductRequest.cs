namespace Catalog.Api.Controllers.Products
{
	public sealed record ProductRequest(string Name, string Description, decimal Price, Guid CategoryId);

}
