using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Detector;

public sealed class DetectorItem : BoundedItem, AssetItem, TagUser
{
	public DomainTag Tag
	{
		get;
		set
		{
			InappropriateTagsOwnerChangeException.ThrowIfOwnerChanged(field, value);
			field = value;
		}
	}

	internal DetectorItem(DomainTag tag)
	{
		Tag = tag;
	}
}