using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stock_Back_End.Models.OrderModels
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
               .HasOne(o => o.Client)
               .WithMany(c => c.Orders)
               .HasForeignKey("ClientId");

            builder.Property(o => o.Date).HasColumnType("datetime").IsRequired();
        }
    }
}
