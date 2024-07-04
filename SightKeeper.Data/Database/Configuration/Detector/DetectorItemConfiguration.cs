using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Database.Configuration.Detector;

public sealed class DetectorItemConfiguration : IEntityTypeConfiguration<DetectorItem>
{
	public void Configure(EntityTypeBuilder<DetectorItem> builder)
	{
		builder.HasFlakeIdKey();
		builder.ComplexProperty(item => item.Bounding, boundingBuilder =>
		{
			boundingBuilder.ComplexProperty(bounding => bounding.Position, positionBuilder =>
			{
				positionBuilder.Property(position => position.X);
				positionBuilder.Property(position => position.Y);
			});
			boundingBuilder.ComplexProperty(bounding => bounding.Size, sizeBuilder =>
			{
				sizeBuilder.Property(position => position.X);
				sizeBuilder.Property(position => position.Y);
			});
		});
		builder.ToTable("DetectorItems");
	}
}