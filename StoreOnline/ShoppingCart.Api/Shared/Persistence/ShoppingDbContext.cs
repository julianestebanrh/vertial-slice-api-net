using Microsoft.EntityFrameworkCore;
using ShoppingCart.Api.Shared.Domain.Entities;

namespace ShoppingCart.Api.Shared.Persistence
{
	public sealed class ShoppingDbContext(DbContextOptions<ShoppingDbContext> options) : DbContext(options)
	{
		public DbSet<Item> Items => Set<Item>();
		public DbSet<Cart> Carts => Set<Cart>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//var cart = Cart.Create("INIT_CODE", "SYSTEM");
			//var items = new List<Item>
			//{
			//	Item.Create("ITEM_CODE", "", 10, 15.00m, "PHONE X", "MODERN", cart.Id),
			//	Item.Create("ITEM_AM", "", 5, 12.00m, "MACBOOK", "POTENT", cart.Id),
			//};

			//var cartEmpty = Cart.Create("INIT_PASS", "SYSTEM");

			//modelBuilder.Entity<Cart>().HasData(cart);
			//modelBuilder.Entity<Item>().HasData(items);
			//modelBuilder.Entity<Cart>().HasData(cartEmpty);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingDbContext).Assembly);
			base.OnModelCreating(modelBuilder);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			foreach (var entry in ChangeTracker.Entries<Entity>())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.CreatedOn = DateTime.UtcNow;
						entry.Entity.CreatedBy = "SYSTEM";
						entry.Entity.LastModifiedOn = DateTime.UtcNow;
						entry.Entity.LastModifiedBy = "SYSTEM";
						break;
					case EntityState.Modified:
						entry.Entity.LastModifiedOn = DateTime.UtcNow;
						entry.Entity.LastModifiedBy = "SYSTEM";
						break;
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
