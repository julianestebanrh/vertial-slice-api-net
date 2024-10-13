using Catalog.Domain.Abstractions;
using Catalog.Domain.Entities.Categories;
using Catalog.Domain.Entities.Products;
using Catalog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<CatalogDbContext>(options =>
			{
				// Imprimir en la consola todas las sentencias SQL enviadas a la base de datos
				options.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information).EnableSensitiveDataLogging();

				options.UseSqlite(configuration.GetConnectionString("Database")).UseSnakeCaseNamingConvention();
			});

			services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<CatalogDbContext>());
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();

			return services;
		}
	}
}
