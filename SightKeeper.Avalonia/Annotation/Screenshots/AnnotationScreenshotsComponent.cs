using System;
using System.Reactive;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

public interface AnnotationScreenshotsComponent : ScreenshotsDataContext
{
	ScreenshotsLibrary? Library { set; }
	IObservable<Unit> SelectedScreenshotChanged { get; }
	Screenshot SelectedScreenshot { get; }
}