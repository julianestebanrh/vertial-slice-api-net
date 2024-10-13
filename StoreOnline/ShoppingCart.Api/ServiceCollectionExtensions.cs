using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Api.Shared.Behaviors;
using ShoppingCart.Api.Shared.Data;
using ShoppingCart.Api.Shared.Exceptions;
using ShoppingCart.Api.Shared.Metrics;
using ShoppingCart.Api.Shared.Networking.Catalog.Api;
using ShoppingCart.Api.Shared.Persistence;
using ShoppingCart.Api.Shared.Slices;
using System.Reflection;

namespace ShoppingCart.Api
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddExceptionHandler<GlobalExceptionHandler>();
			services.AddProblemDetails();

			var currentAssembly = Assembly.GetExecutingAssembly();
			services.AddAutoMapper(currentAssembly);

			services.AddMediatR(options =>
			{
				options.RegisterServicesFromAssemblies(currentAssembly)
					.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>))
					.AddOpenBehavior(typeof(ValidatorBehavior<,>))
					.AddOpenBehavior(typeof(PerformanceBehavior<,>));
			});

			services.AddValidatorsFromAssembly(currentAssembly);

			services.AddSingleton<HandlerPerformance>();

			services.AddScoped<ICatalogApiClient, CatalogApiClient>();

			return services;
		}

		public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Database")
					?? throw new ArgumentNullException(nameof(configuration));

			services.AddDbContext<ShoppingDbContext>(options =>
			{
				options.LogTo(Console.WriteLine, new[]
				{
					DbLoggerCategory.Database.Command.Name,
				}, LogLevel.Information).EnableSensitiveDataLogging();

				options.UseSqlite(connectionString).UseSnakeCaseNamingConvention();
			});

			services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
			SqlMapper.AddTypeHandler(new GuidOnlyTypeHandler());

			return services;
		}

		public static IServiceCollection AddEndpoints(this IServiceCollection services)
		{
			var currentAssembly = Assembly.GetExecutingAssembly();

			var endpoints = currentAssembly.GetTypes().Where(x => typeof(IEndpoint).IsAssignableFrom(x) && x != typeof(IEndpoint) && x.IsPublic && !x.IsAbstract);
			foreach (var endpoint in endpoints)
			{
				services.AddSingleton(typeof(IEndpoint), endpoint);
			}

			return services;
		}
	}
}
