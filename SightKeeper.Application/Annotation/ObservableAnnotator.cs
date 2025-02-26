using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Annotation;

public interface ObservableAnnotator
{
	IObservable<Image> AssetsChanged { get; }
}