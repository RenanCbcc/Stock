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

        }
    }
}
