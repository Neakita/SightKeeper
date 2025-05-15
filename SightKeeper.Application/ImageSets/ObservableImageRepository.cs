using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets;

public interface ObservableImageRepository : ObservableRepository<Image>
{
	public IObservable<ImagesRange> ImagesDeleted { get; }
}