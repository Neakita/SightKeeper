using System;
using System.Reactive;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

public interface ScreenshotSelection
{
	int SelectedScreenshotIndex { get; }
	IObservable<Unit> SelectedScreenshotChanged { get; }
	ScreenshotsLibrary? Library { get; }
	Image? SelectedImage => SelectedScreenshotIndex >= 0 ? Library?.Screenshots[SelectedScreenshotIndex] : null;
}