using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Images;

public interface ImagesDataContext
{
	IReadOnlyCollection<AnnotationImageDataContext> Images { get; }
	int SelectedImageIndex { get; set; }
}