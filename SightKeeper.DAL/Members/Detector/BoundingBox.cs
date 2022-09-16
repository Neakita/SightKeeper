using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.DAL.Members.Detector;

public sealed class BoundingBox
{
	public float X { get; }
	public float Y { get; }
	public float Width { get; }
	public float Height { get; }
	
	
	public BoundingBox(float x, float y, float width, float height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}
}

internal sealed class BoundingBoxConfiguration : IEntityTypeConfiguration<BoundingBox>
{
	public void Configure(EntityTypeBuilder<BoundingBox> builder)
	{
		builder.Property(boundingBox => boundingBox.X);
		builder.Property(boundingBox => boundingBox.Y);
		builder.Property(boundingBox => boundingBox.Width);
		builder.Property(boundingBox => boundingBox.Height);
	}
}