using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .ValueGeneratedOnAdd();

            // شماره فاکتور
            builder.Property(i => i.Number)
                .HasConversion(
                    num => num.Value,
                    value => InvoiceNumber.CreateUnsafe(value))
                .HasColumnName("InvoiceNumber")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.CustomerId)
                .IsRequired();

            builder.Property(i => i.IssuedAtUtc)
                .IsRequired();

            // مبلغ کل
            builder.Property(i => i.TotalAmount)
                .HasConversion(
                    m => m.Value,
                    val => Amount.CreateUnsafe(val))
                .HasColumnName("TotalAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.OwnsMany(i => i.Items, itemBuilder =>
            {
                itemBuilder.ToTable("InvoiceItems");

                itemBuilder.WithOwner().HasForeignKey(item => item.InvoiceId);
                itemBuilder.HasKey(item => item.Id);
                itemBuilder.Property(item => item.Id)
                    .ValueGeneratedOnAdd();

                itemBuilder.Property(item => item.ProductId)
                    .IsRequired();

                // نام کالا (Namespace اصلاح شد)
                itemBuilder.Property(item => item.ProductName)
                    .HasConversion(
                        name => name.Value,
                        val => ProductName.CreateUnsafe(val))
                    .HasColumnName("ProductName")
                    .HasMaxLength(200)
                    .IsRequired();

                // قیمت واحد
                itemBuilder.Property(item => item.UnitPrice)
                    .HasConversion(
                        p => p.Value,
                        val => Amount.CreateUnsafe(val))
                    .HasColumnName("UnitPrice")
                    .HasPrecision(18, 2)
                    .IsRequired();

                // تعداد
                itemBuilder.Property(item => item.Quantity)
                    .HasConversion(
                        q => q.Value,
                        val => InvoiceQuantity.CreateUnsafe(val))
                    .HasColumnName("Quantity")
                    .IsRequired();

                // مبلغ کل ردیف
                itemBuilder.Property(item => item.TotalPrice)
                    .HasConversion(
                        tp => tp.Value,
                        val => Amount.CreateUnsafe(val))
                    .HasColumnName("TotalPrice")
                    .HasPrecision(18, 2)
                    .IsRequired();
            });

            
            builder.Metadata.FindNavigation(nameof(Invoice.Items))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(i => i.Number).IsUnique();
            builder.Ignore(i => i.DomainEvents);
        }
    }
}
