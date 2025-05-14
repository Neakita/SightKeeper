using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets;

public interface ObservableImageDataAccess : ObservableDataAccess<Image>
{
	public IObservable<ImagesRange> ImagesDeleted { get; }
}