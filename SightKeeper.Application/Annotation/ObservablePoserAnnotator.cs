using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Application.Annotation;

public interface ObservablePoserAnnotator
{
	IObservable<(DomainPoserItem item, DomainKeyPoint keyPoint)> KeyPointCreated { get; }
	IObservable<(DomainPoserItem item, DomainKeyPoint keyPoint)> KeyPointDeleted { get; }
}