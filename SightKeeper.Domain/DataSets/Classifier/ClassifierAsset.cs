using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Classifier;

public sealed class ClassifierAsset : Asset
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

	internal ClassifierAsset(Tag tag)
	{
		_tag = tag;
	}

	private Tag _tag;
}