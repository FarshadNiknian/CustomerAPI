using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            // نام محصول
            builder.Property(p => p.Name)
                .HasConversion(
                    name => name.Value,
                    value => ProductName.CreateUnsafe(value))
                .HasColumnName("Name")
                .HasMaxLength(200)
                .IsRequired();

            // کد محصول
            builder.Property(p => p.Code)
                .HasConversion(
                    code => code.Value,
                    value => ProductCode.CreateUnsafe(value))
                .HasColumnName("Code")
                .HasMaxLength(50)
                .IsRequired();

            // موجودی انبار
            builder.Property(p => p.Count)
                .HasConversion(
                    count => count.Value,
                    value => ProductCount.CreateUnsafe(value))
                .HasColumnName("Count")
                .IsRequired();

            //حذف منطقی
            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasQueryFilter(p => !p.IsDeleted);

            builder.HasIndex(p => p.Code).IsUnique();
            builder.Ignore(p => p.DomainEvents);
        }
    }
}
