using System;

namespace SightKeeper.Avalonia.Annotation.Tooling.ImageSet;

public interface ImageSetSelection
{
	Domain.Images.ImageSet? SelectedImageSet { get; }
	IObservable<Domain.Images.ImageSet?> SelectedImageSetChanged { get; }
}