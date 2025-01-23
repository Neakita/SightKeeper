using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Application.Annotation;

public interface ObservablePoserAnnotator
{
	IObservable<(PoserItem item, KeyPoint keyPoint)> KeyPointCreated { get; }
	IObservable<(PoserItem item, KeyPoint keyPoint)> KeyPointDeleted { get; }
}