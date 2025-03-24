using SightKeeper.Domain.Images;
using Vibrance;

namespace SightKeeper.Avalonia.Annotation.Images;

public interface ImagesDataContext
{
	ReadOnlyObservableList<Image> Images { get; }
	int SelectedImageIndex { get; set; }
}