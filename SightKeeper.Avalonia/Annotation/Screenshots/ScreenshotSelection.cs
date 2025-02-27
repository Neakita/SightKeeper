using System;
using System.Reactive;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

public interface ScreenshotSelection
{
	int SelectedScreenshotIndex { get; }
	IObservable<Unit> SelectedScreenshotChanged { get; }
	ImageSet? Set { get; }
	Image? SelectedImage => SelectedScreenshotIndex >= 0 ? Set?.Images[SelectedScreenshotIndex] : null;
}