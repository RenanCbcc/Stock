using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Stock_Back_End.Models.PaymentModels
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder
                .HasOne(p => p.Client)
                .WithMany(c => c.Payments)
                .HasForeignKey("ClientId");

            builder.Property(p => p.Date)
                .HasColumnType("datetime")
               .HasDefaultValueSql("getdate()");
        }
    }
}
