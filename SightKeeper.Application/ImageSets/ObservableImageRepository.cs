namespace SightKeeper.Application.ImageSets;

public interface ObservableImageRepository : ObservableRepository<DomainImage>
{
	public IObservable<ImagesRange> ImagesDeleted { get; }
}