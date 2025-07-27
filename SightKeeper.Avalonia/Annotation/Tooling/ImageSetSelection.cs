using System;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface ImageSetSelection
{
	ImageSet? SelectedImageSet { get; }
	IObservable<ImageSet?> SelectedImageSetChanged { get; }
}