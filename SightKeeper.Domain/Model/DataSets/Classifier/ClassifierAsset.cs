using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierAsset : Asset
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

	internal ClassifierAsset(Tag tag)
	{
		Tag = tag;
	}
}