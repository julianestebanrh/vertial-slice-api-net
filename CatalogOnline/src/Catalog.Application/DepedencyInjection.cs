using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application
{
	public static class DepedencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddMediatR(config =>
			{
				config.RegisterServicesFromAssembly(typeof(DepedencyInjection).Assembly);
			});

			services.AddValidatorsFromAssembly(typeof(DepedencyInjection).Assembly);

			return services;
		}
	}
}
