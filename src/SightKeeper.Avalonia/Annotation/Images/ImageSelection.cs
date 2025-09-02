using System;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Images;

public interface ImageSelection
{
	int SelectedImageIndex { get; }
	IObservable<ManagedImage?> SelectedImageChanged { get; }
	ManagedImage? SelectedImage { get; }
}