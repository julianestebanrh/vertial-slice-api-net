using MediatR.Pipeline;

namespace ShoppingCart.Api.Shared.Behaviors
{
	public sealed class LoggingBehavior<TRequest>(ILogger<TRequest> logger) : IRequestPreProcessor<TRequest> where TRequest : notnull
	{
		private readonly ILogger<TRequest> _logger = logger;
		public Task Process(TRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Running the functionality: {featureRequestName}", typeof(TRequest).Name);

			return Task.CompletedTask;
		}
	}
}
