using Polly;
using Polly.Registry;

namespace ShoppingCart.Api.Shared.Networking.Catalog.Api
{
	public sealed class CatalogApiClient(CatalogApiService catalogApiService, ILoggerFactory loggerFactory, ResiliencePipelineProvider<string> pipelineProvider) : ICatalogApiClient
	{
		private readonly ILoggerFactory _loggerFactory = loggerFactory;
		private readonly ResiliencePipelineProvider<string> _pipelineProvider = pipelineProvider;
		private readonly CatalogApiService _catalogApiService = catalogApiService;

		public async Task<Domain.Models.Catalog?> GetProductByCodeAsync(string code, CancellationToken cancellationToken)
		{
			var _logger = _loggerFactory.CreateLogger("RetryLog");

			var policy = Policy.Handle<ApplicationException>().WaitAndRetryAsync(3, retryAttemp =>
			{
				_logger.LogInformation($"Intent: {retryAttemp}");
				var timeToRetry = TimeSpan.FromSeconds(Math.Pow(2, retryAttemp));
				return timeToRetry;
			});

			var product = await policy.ExecuteAsync(() => _catalogApiService.GetProductByCodeAsync(code, cancellationToken));

			return product;
		}

		public async Task<IEnumerable<Domain.Models.Catalog>> GetProductsAsync(CancellationToken cancellationToken)
		{
			var _logger = _loggerFactory.CreateLogger("RetryLog");
			//var policy = Policy.Handle<ApplicationException>().WaitAndRetryAsync(3, retryAttemp =>
			//{
			//	_logger.LogInformation($"Intent: {retryAttemp}");
			//	var timeToRetry = TimeSpan.FromSeconds(Math.Pow(2, retryAttemp));
			//	return timeToRetry;
			//});

			//var products = await policy.ExecuteAsync(() => _catalogApiService.GetProductsAsync(cancellationToken));

			var pipeline = _pipelineProvider.GetPipeline<IEnumerable<Domain.Models.Catalog>>("catalog-products");

			var products = await pipeline.ExecuteAsync(async token => await _catalogApiService.GetProductsAsync(cancellationToken), cancellationToken);

			return products;
		}
	}
}
