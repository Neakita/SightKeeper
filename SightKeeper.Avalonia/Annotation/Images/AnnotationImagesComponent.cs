using System;
using System.Reactive;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Images;

public interface AnnotationImagesComponent : ImagesDataContext
{
	ImageSet? Set { set; }
	IObservable<Unit> SelectedImageChanged { get; }
	Image? SelectedImage { get; }
}