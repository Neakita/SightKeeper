using System;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Images;

public interface ImageSelection
{
	int SelectedImageIndex { get; }
	IObservable<Image?> SelectedImageChanged { get; }
	ImageSet? Set { get; }
	Image? SelectedImage => SelectedImageIndex >= 0 ? Set?.Images[SelectedImageIndex] : null;
}