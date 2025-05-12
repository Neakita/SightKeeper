using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Application.Annotation;

public interface ObservableBoundingAnnotator
{
	IObservable<(ItemsMaker<AssetItem> asset, AssetItem item)> ItemCreated { get; }
}