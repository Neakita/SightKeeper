using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier.Assets.Decorators;

internal sealed class LockingClassifierAsset(ClassifierAsset inner, Lock editingLock) : ClassifierAsset, Decorator<ClassifierAsset>
{
	public ManagedImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set
		{
			lock (editingLock)
				inner.Usage = value;
		}
	}

	public Tag Tag
	{
		get => inner.Tag;
		set
		{
			lock (editingLock)
				inner.Tag = value;
		}
	}

	public ClassifierAsset Inner => inner;
}