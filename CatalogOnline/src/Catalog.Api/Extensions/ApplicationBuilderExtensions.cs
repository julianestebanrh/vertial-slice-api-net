using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		public static async void ApplyMigration(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var service = scope.ServiceProvider;
				var loggerFactory = service.GetRequiredService<ILoggerFactory>();

				try
				{
					var context = service.GetRequiredService<CatalogDbContext>();
					await context.Database.MigrateAsync();
				}
				catch (Exception ex)
				{
					var logger = loggerFactory.CreateLogger<Program>();
					logger.LogError(ex, "Error migration");
				}

			}
		}
	}
}
