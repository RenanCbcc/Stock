using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stock_Back_End.Models.ProductModels
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Description).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Code).HasMaxLength(13).IsRequired();
            builder.HasAlternateKey(p => p.Code);
            
            builder.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey("CategoryId");
            builder.HasOne(p => p.Supplier).WithMany(c => c.Products).HasForeignKey("SupplierId");
        }
    }
}
