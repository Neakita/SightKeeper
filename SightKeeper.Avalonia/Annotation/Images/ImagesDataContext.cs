using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Images;

public interface ImagesDataContext
{
	IReadOnlyCollection<ImageViewModel> Images { get; }
	int SelectedImageIndex { get; set; }
}