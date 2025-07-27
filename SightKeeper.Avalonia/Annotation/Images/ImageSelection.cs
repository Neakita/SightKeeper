using System;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Images;

public interface ImageSelection
{
	int SelectedImageIndex { get; }
	IObservable<Image?> SelectedImageChanged { get; }
	Image? SelectedImage { get; }
}