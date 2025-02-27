using System;
using System.Reactive;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

public interface AnnotationScreenshotsComponent : ScreenshotsDataContext
{
	ImageSet? Set { set; }
	IObservable<Unit> SelectedScreenshotChanged { get; }
	Image SelectedImage { get; }
}