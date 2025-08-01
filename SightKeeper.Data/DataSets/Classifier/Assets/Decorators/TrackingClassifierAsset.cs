using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Classifier.Assets.Decorators;

internal sealed class TrackingClassifierAsset(StorableClassifierAsset inner, ChangeListener changeListener) : StorableClassifierAsset
{
	public StorableImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set
		{
			inner.Usage = value;
			changeListener.SetDataChanged();
		}
	}

	public StorableTag Tag
	{
		get => inner.Tag;
		set
		{
			inner.Tag = value;
			changeListener.SetDataChanged();
		}
	}
}