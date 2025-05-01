using SightKeeper.Domain.Images;
using Range = Vibrance.Utilities.Range;

namespace SightKeeper.Application.ImageSets;

public interface ObservableImageDataAccess : ObservableDataAccess<Image>
{
	public IObservable<(ImageSet Set, Range Range)> DeletingImages { get; }
}