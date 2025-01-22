using System;
using System.Reactive;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

public interface ScreenshotSelection
{
	int SelectedScreenshotIndex { get; }
	IObservable<Unit> SelectedScreenshotChanged { get; }
	ScreenshotsLibrary? Library { get; }
	Screenshot? SelectedScreenshot => SelectedScreenshotIndex >= 0 ? Library?.Screenshots[SelectedScreenshotIndex] : null;
}