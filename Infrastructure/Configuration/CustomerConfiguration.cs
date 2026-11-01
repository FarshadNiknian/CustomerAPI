using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Configuration
{
    public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.OwnsOne(c => c.CustomerName, nameBuilder =>
            {
                nameBuilder.Property(n => n.FirstName)
                    .HasColumnName("FirstName")
                    .HasMaxLength(100)
                    .IsRequired(true);

                nameBuilder.Property(n => n.LastName)
                    .HasColumnName("LastName")
                    .HasMaxLength(100)
                    .IsRequired(true);
            });
           
            builder.Property(c => c.NationalCode)
                .HasConversion(
                    code => code.Value,
                    value => NationalCode.CreateUnsafe(value))
                .HasColumnName("NationalCode")
                .HasMaxLength(10)
                .IsFixedLength()
                .IsRequired(true);

            builder.HasIndex(c => c.NationalCode)
                .IsUnique();

            builder.Ignore(c => c.DomainEvents);
        }
    }
}

