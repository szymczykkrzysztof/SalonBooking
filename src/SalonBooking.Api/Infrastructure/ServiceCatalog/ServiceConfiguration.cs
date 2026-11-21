using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonBooking.Api.Domain.ServiceCatalog;

namespace SalonBooking.Api.Infrastructure.ServiceCatalog;

internal sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("services");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).HasMaxLength(Service.NameMaxLength).IsRequired();
        builder.Property(s => s.Category).HasConversion<string>().HasMaxLength(30);
        builder.OwnsOne(s => s.DefaultPrice, price =>
        {
            price.Property(p => p.Amount).HasColumnName("price_amount").HasPrecision(10, 2);
            price.Property(p => p.Currency).HasColumnName("price_currency").HasMaxLength(3);
        });
    }
}