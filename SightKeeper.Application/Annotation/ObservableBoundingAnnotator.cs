using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Application.Annotation;

public interface ObservableBoundingAnnotator
{
	IObservable<(ItemsCreator asset, BoundedItem item)> ItemCreated { get; }
}