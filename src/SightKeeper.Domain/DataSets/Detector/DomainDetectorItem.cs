using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Detector;

public sealed class DomainDetectorItem(DetectorItem inner) : DetectorItem
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
}