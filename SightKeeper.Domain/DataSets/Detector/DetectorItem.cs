using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Detector;

public sealed class DetectorItem : AssetItem
{
	public Tag Tag
	{
		get => _tag;
		set
		{
			InappropriateTagsOwnerChangeException.ThrowIfOwnerChanged(_tag, value);
			_tag = value;
		}
	}

	internal DetectorItem(Bounding bounding, Tag tag) : base(bounding)
	{
		_tag = tag;
	}

	private Tag _tag;
}