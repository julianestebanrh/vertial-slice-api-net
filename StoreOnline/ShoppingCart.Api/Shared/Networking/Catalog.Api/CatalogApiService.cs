using Newtonsoft.Json;

namespace ShoppingCart.Api.Shared.Networking.Catalog.Api
{
	public sealed class CatalogApiService(HttpClient httpClient)
	{
		private readonly HttpClient _httpClient = httpClient;

		public async Task<IEnumerable<Domain.Models.Catalog>> GetProductsAsync(CancellationToken cancellationToken)
		{
			var content = await _httpClient.GetFromJsonAsync<IEnumerable<Domain.Models.Catalog>>("/api/products", cancellationToken);

			return content!;
		}

		public async Task<Domain.Models.Catalog> GetProductByCodeAsync(string code, CancellationToken cancellationToken)
		{
			//var content = await _httpClient.GetFromJsonAsync<Domain.Models.Catalog>($"/api/products/code/{code}", cancellationToken);

			var response = await _httpClient.GetAsync($"api/products/code/{code}", cancellationToken);

			if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
			{
				return null;
			}

			var contentString = await response.Content.ReadAsStringAsync();
			var content = JsonConvert.DeserializeObject<Domain.Models.Catalog>(contentString);
			return content!;
		}

	}
}
