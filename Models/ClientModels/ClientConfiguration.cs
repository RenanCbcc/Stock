using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Stock_Back_End.Models.ClientModels
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(c => c.LastPurchase).HasColumnType("datetime");
            builder.Property(c => c.Debt).IsRequired();
            builder.Property(c => c.Status).IsRequired();
            builder.Property(c => c.Name).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Address).HasMaxLength(100).IsRequired();
            builder.Property(c => c.PhoneNumber).HasMaxLength(11).IsRequired();


        }
    }
}
