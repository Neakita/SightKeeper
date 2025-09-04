using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Classifier.Assets.Decorators;

internal sealed class TrackingClassifierAsset(ClassifierAsset inner, ChangeListener changeListener) : ClassifierAsset, Decorator<ClassifierAsset>
{
	public ManagedImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set
		{
			inner.Usage = value;
			changeListener.SetDataChanged();
		}
	}

	public Tag Tag
	{
		get => inner.Tag;
		set
		{
			inner.Tag = value;
			changeListener.SetDataChanged();
		}
	}

	public ClassifierAsset Inner => inner;
}