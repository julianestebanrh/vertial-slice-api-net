using System.Diagnostics.Metrics;

namespace ShoppingCart.Api.Shared.Metrics
{
	public sealed class HandlerPerformance
	{
		private readonly Counter<long> _milliSecondsElapsed;

		public HandlerPerformance(IMeterFactory meterFactory)
		{
			var meter = meterFactory.Create("ShoppingCart.Api");
			_milliSecondsElapsed = meter.CreateCounter<long>(
				"shippingcart.api.requesthandler.millisecondselapsed");
		}

		public void MilliSecondsElapsed(long milliseconds) => _milliSecondsElapsed.Add(milliseconds);
	}
}
