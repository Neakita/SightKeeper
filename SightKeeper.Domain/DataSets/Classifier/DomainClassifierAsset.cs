using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Classifier;

public sealed class DomainClassifierAsset(ClassifierAsset inner) : ClassifierAsset
{
	public ManagedImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set => inner.Usage = value;
	}

	public Tag Tag
	{
		get => inner.Tag;
		set
		{
			InappropriateTagsOwnerChangeException.ThrowIfOwnerChanged(Tag, value);
			inner.Tag = value;
		}
	}
}