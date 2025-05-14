using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Detector;

public sealed class DetectorItem : BoundedItem, AssetItem, TagUser
{
	public Tag Tag
	{
		get => _tag;
		set
		{
			InappropriateTagsOwnerChangeException.ThrowIfOwnerChanged(_tag, value);
			_tag.RemoveUser(this);
			_tag = value;
			_tag.AddUser(this);
		}
	}

	internal DetectorItem(Tag tag)
	{
		_tag = tag;
		_tag.AddUser(this);
	}

	private Tag _tag;
}