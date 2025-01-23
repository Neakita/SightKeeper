using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Annotation;

public interface ObservableBoundingAnnotator
{
	IObservable<(ItemsCreator asset, BoundedItem item)> ItemCreated { get; }
}