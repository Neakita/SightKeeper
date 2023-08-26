using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Configuration;

public sealed class DetectorItemConfiguration : IEntityTypeConfiguration<DetectorItem>
{
    public void Configure(EntityTypeBuilder<DetectorItem> builder)
    {
        builder.ToTable("DetectorItems");
        builder.HasShadowKey();
        builder.OwnsOne(item => item.Bounding, boundingBuilder =>
        {
            boundingBuilder.Property(bounding => bounding.Left).HasColumnName("BoundingLeft");
            boundingBuilder.Property(bounding => bounding.Top).HasColumnName("BoundingTop");
            boundingBuilder.Property(bounding => bounding.Right).HasColumnName("BoundingRight");
            boundingBuilder.Property(bounding => bounding.Bottom).HasColumnName("BoundingBottom");
        });
    }
}