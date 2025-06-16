using System;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Images;

public interface ImageSelection
{
	int SelectedImageIndex { get; }
	IObservable<DomainImage?> SelectedImageChanged { get; }
	DomainImageSet? Set { get; }
	DomainImage? SelectedImage => SelectedImageIndex >= 0 ? Set?.Images[SelectedImageIndex] : null;
}