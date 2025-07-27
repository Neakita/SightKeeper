using System;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface ImageSetSelection
{
	IObservable<ImageSet?> SelectedImageSetChanged { get; }
}