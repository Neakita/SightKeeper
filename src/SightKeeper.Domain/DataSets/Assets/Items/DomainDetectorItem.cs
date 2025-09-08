using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public sealed class DomainDetectorItem(DetectorItem inner) : DetectorItem, Decorator<DetectorItem>
{
	public Tag Tag
	{
		get => inner.Tag;
		set
		{
			InappropriateTagsOwnerChangeException.ThrowIfOwnerChanged(Tag, value);
			inner.Tag = value;
		}
	}

	public Bounding Bounding
	{
		get => inner.Bounding;
		set
		{
			ItemBoundingConstraintException.ThrowIfNotNormalized(this, value);
			inner.Bounding = value;
		}
	}

	public DetectorItem Inner => inner;
}