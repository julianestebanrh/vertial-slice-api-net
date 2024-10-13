using Catalog.Domain.Entities.Categories;
using Catalog.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Configurations
{
	internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.ToTable("products");

			builder.HasKey(x => x.Id);

			builder.HasOne<Category>()
				.WithMany()
				.HasForeignKey(x => x.CategoryId);
		}
	}
}
