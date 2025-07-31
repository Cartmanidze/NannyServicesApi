using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NannyServices.Domain.Entities;

namespace NannyServices.Infrastructure.Data.Configurations;

public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.HasKey(ol => ol.Id);

        builder.Property(ol => ol.ProductId)
            .IsRequired();

        builder.Property(ol => ol.Count)
            .IsRequired();

        builder.Property(ol => ol.OrderId)
            .IsRequired();
        
        builder.OwnsOne(ol => ol.Price, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasColumnName("PriceAmount");

            priceBuilder.Property(m => m.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("PriceCurrency");
        });
        
        builder.HasOne(ol => ol.Product)
            .WithMany()
            .HasForeignKey(ol => ol.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(ol => ol.CreatedAt)
            .IsRequired();

        builder.Property(ol => ol.UpdatedAt);

        builder.HasIndex(ol => ol.OrderId);
        builder.HasIndex(ol => ol.ProductId);
    }
}