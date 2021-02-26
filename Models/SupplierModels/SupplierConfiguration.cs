using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stock_Back_End.Models.SupplierModels
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.Property(s => s.Name).HasMaxLength(50).IsRequired();
            builder.Property(s => s.Email).HasMaxLength(50).IsRequired();
            builder.Property(s => s.PhoneNumber).HasMaxLength(11).IsRequired();
            builder.HasAlternateKey(s => s.Name);
            
        }
    }
}
