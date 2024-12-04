using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorItem : AssetItem
{
	public Tag Tag
	{
		get;
		set
		{
			InappropriateTagsOwnerChangeException.ThrowIfOwnerChanged(field, value);
			field = value;
		}
	}

	internal DetectorItem(Bounding bounding, Tag tag) : base(bounding)
	{
		Tag = tag;
	}
}