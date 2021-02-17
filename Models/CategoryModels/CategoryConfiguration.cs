using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Stock_Back_End.Models.CategoryModels
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(c => c.Products).WithOne(p => p.Category).HasForeignKey("CategoryId");
            builder.Property(c => c.Title).HasMaxLength(25).IsRequired();
        }
    }
}
