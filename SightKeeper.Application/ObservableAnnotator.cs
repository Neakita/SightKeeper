using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application;

public interface ObservableAnnotator
{
	IObservable<Screenshot> AssetsChanged { get; }
}