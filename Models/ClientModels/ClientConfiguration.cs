using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Stock_Back_End.Models.ClientModels
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(c => c.Name).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Address).HasMaxLength(100).IsRequired();
            builder.Property(c => c.PhoneNumber).HasMaxLength(11).IsRequired();
            builder.Property(c => c.LastPurchase)
               .HasColumnType("datetime")
               .HasDefaultValueSql("getdate()");
            builder.Property(c => c.Status)
                .HasMaxLength(20)
                .IsRequired()
                .HasConversion<string>(e => e.ToString(), e => (Status)Enum.Parse(typeof(Status), e)
                );

        }
    }
}
