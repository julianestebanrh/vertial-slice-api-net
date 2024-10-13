using Microsoft.EntityFrameworkCore;
using ShoppingCart.Api.Shared.Domain.Entities;
using ShoppingCart.Api.Shared.Persistence;

namespace ShoppingCart.Api.Shared.Extensions
{
	public static class MigrationExtension
	{
		public static async Task ApplyMigration(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var service = scope.ServiceProvider;
				var loggerFactory = service.GetRequiredService<ILoggerFactory>();

				try
				{
					var context = service.GetRequiredService<ShoppingDbContext>();
					await context.Database.MigrateAsync();

					await SeedData(context);
				}
				catch (Exception ex)
				{
					var logger = loggerFactory.CreateLogger<Program>();
					logger.LogError(ex, "Migration error");
				}
			}
		}

		public static async Task SeedData(ShoppingDbContext context)
		{
			if (!context.Carts.Any())
			{
				var shoppingCart = CartBuilder
										.Create()
										.SetCreatedOn(DateTime.UtcNow)
										.SetCode("ITEM_CODE")
										.SetUserId("SYSTEM")
										.AddItem(Item.Create("ITEM_CODE", "", 10, 15.00m, "PHONE X", "MODERN"))
										.AddItem(Item.Create("ITEM_AM", "", 5, 12.00m, "MACBOOK", "POTENT"))
										.Build(Guid.NewGuid());

				context.Add(shoppingCart);

				var shoppingCartEmpty = CartBuilder
										.Create()
										.SetCreatedOn(DateTime.UtcNow)
										.SetCode("INIT_PASS")
										.SetUserId("SYSTEM")
										.Build(Guid.NewGuid());

				context.Add(shoppingCartEmpty);

				await context.SaveChangesAsync();
			}
		}
	}
}
