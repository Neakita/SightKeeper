using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Detector;

public sealed class DetectorItem
{
	public Guid Id { get; }
	public ItemClass ItemClass { get; private set; }
	public BoundingBox BoundingBox { get; private set; }
	
	
	public DetectorItem(ItemClass itemClass, BoundingBox boundingBox)
	{
		ItemClass = itemClass;
		BoundingBox = boundingBox;
	}


	private DetectorItem(Guid id)
	{
		Id = id;
		ItemClass = null!;
		BoundingBox = null!;
	}
}

internal sealed class DetectorItemConfiguration : IEntityTypeConfiguration<DetectorItem>
{
	public void Configure(EntityTypeBuilder<DetectorItem> builder)
	{
		builder.HasKey(item => item.Id);
		builder.OwnsOne(item => item.BoundingBox);
	}
}