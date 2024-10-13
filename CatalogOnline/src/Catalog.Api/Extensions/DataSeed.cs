using Bogus;
using Catalog.Domain.Entities.Categories;
using Catalog.Domain.Entities.Products;
using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Extensions
{
	public static class DataSeed
	{
		public static async Task SeedCatalogProduct(this IApplicationBuilder app)
		{
			using var scope = app.ApplicationServices.CreateScope();
			var service = scope.ServiceProvider;
			var loggerFactory = service.GetRequiredService<ILoggerFactory>();

			try
			{
				var context = service.GetRequiredService<CatalogDbContext>();

				if (!context.Set<Category>().Any())
				{
					var categoryComputer = Category.Create("Computer");
					var categoryTelephone = Category.Create("Telephone");
					context.AddRange(new List<Category> { categoryComputer, categoryTelephone });
					await context.SaveChangesAsync();
				}

				if (!context.Set<Product>().Any())
				{
					var computer = await context.Set<Category>().Where(x => x.Name == "Computer").FirstOrDefaultAsync();
					var telephone = await context.Set<Category>().Where(x => x.Name == "Telephone").FirstOrDefaultAsync();

					var faker = new Faker();
					List<Product> products = new List<Product>();
					var defaultValue = 10000;
					for (var i = 1; i <= 10; i++)
					{
						products.Add(
							Product.Create(
								faker.Commerce.Product(),
								100.00m,
								faker.Commerce.ProductDescription(),
								$"img_{i}.jpg",
								faker.Hashids.Encode(defaultValue, i * 1000), i > 5 ? computer!.Id : telephone!.Id
								)
							);
					}
					context.AddRange(products);
					await context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<CatalogDbContext>();
				logger.LogError(ex, ex.Message);
			}
		}
	}
}
