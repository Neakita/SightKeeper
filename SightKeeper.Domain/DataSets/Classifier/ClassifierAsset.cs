using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Classifier;

public sealed class ClassifierAsset : Asset, TagUser
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

	internal ClassifierAsset(Tag tag)
	{
		_tag = tag;
		_tag.AddUser(this);
	}

	private Tag _tag;
}