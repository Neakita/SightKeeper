namespace SightKeeper.Application.Annotation;

public interface ObservableAnnotator
{
	IObservable<DomainImage> AssetsChanged { get; }
}