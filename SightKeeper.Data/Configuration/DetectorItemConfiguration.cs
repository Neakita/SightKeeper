using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

public sealed class DetectorItemConfiguration : IEntityTypeConfiguration<DetectorItem>
{
    public void Configure(EntityTypeBuilder<DetectorItem> builder)
    {
        builder.HasFlakeIdKey();
        builder.ToTable("DetectorItems");
        builder.ComplexProperty(item => item.Bounding, boundingBuilder =>
        {
	        boundingBuilder.Property(bounding => bounding.Position.X).HasColumnName("BoundingXPosition");
	        boundingBuilder.Property(bounding => bounding.Position.Y).HasColumnName("BoundingYPosition");
            boundingBuilder.Property(bounding => bounding.Size.X).HasColumnName("BoundingXSize");
            boundingBuilder.Property(bounding => bounding.Size.Y).HasColumnName("BoundingYSize");
        });
    }
}