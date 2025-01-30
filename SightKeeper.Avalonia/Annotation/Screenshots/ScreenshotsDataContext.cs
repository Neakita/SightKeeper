using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

public interface ScreenshotsDataContext
{
	IReadOnlyCollection<ScreenshotViewModel> Screenshots { get; }
	int SelectedScreenshotIndex { get; set; }
}