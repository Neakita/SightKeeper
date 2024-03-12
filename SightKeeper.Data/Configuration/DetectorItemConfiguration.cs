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
	        boundingBuilder.ComplexProperty(bounding => bounding.Position, positionBuilder =>
	        {
		        positionBuilder.Property(position => position.X).HasColumnName("BoundingXPosition");
		        positionBuilder.Property(position => position.Y).HasColumnName("BoundingYPosition");
	        });
	        boundingBuilder.ComplexProperty(bounding => bounding.Size, sizeBuilder =>
	        {
		        sizeBuilder.Property(size => size.X).HasColumnName("BoundingXSize");
		        sizeBuilder.Property(size => size.Y).HasColumnName("BoundingYSize");
	        });
        });
    }
}