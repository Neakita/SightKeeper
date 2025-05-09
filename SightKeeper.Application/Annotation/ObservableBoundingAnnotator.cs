using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Application.Annotation;

public interface ObservableBoundingAnnotator
{
	IObservable<(ItemsMaker asset, BoundedItem item)> ItemCreated { get; }
}