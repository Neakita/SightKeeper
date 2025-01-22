using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application.Annotation;

public interface ObservableAnnotator
{
	IObservable<Screenshot> AssetsChanged { get; }
}