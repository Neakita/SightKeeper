using Vibrance;

namespace SightKeeper.Avalonia.Annotation.Images;

public interface ImagesDataContext
{
	ReadOnlyObservableList<ImageViewModel> Images { get; }
	int SelectedImageIndex { get; set; }
}