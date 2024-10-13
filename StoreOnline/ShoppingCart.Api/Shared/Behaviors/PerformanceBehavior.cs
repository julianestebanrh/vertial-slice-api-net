using MediatR;
using ShoppingCart.Api.Shared.Metrics;
using System.Diagnostics;

namespace ShoppingCart.Api.Shared.Behaviors
{
	public sealed class PerformanceBehavior<TRequest, TResponse>(HandlerPerformance handlerPerformance) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly Stopwatch _timer = new();
		private readonly HandlerPerformance _handlerPerformance = handlerPerformance;

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			_timer.Start();
			var response = await next();
			_timer.Stop();
			_handlerPerformance.MilliSecondsElapsed(_timer.ElapsedMilliseconds);
			return response;
		}
	}
}
