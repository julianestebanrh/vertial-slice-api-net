namespace ShoppingCart.Api.Shared.Slices
{
	public static class EndpointRouteBuilderExtension
	{
		public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
		{

			foreach (IEndpoint endpoint in endpointRouteBuilder.ServiceProvider.GetServices<IEndpoint>())
			{
				endpoint.AddEndpoint(endpointRouteBuilder);
			}

			return endpointRouteBuilder;
		}
	}
}
