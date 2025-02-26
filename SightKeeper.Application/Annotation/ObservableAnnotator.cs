using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Annotation;

public interface ObservableAnnotator
{
	IObservable<Screenshot> AssetsChanged { get; }
}