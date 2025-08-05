using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Classifier.Assets.Decorators;

internal sealed class LockingClassifierAsset(StorableClassifierAsset inner, Lock editingLock) : StorableClassifierAsset
{
	public StorableImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set
		{
			lock (editingLock)
				inner.Usage = value;
		}
	}

	public StorableTag Tag
	{
		get => inner.Tag;
		set
		{
			lock (editingLock)
				inner.Tag = value;
		}
	}

	public StorableClassifierAsset Innermost => inner.Innermost;
}