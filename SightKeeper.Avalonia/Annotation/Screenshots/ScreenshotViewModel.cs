using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal sealed class ScreenshotViewModel : ViewModel
{
	public Screenshot Value { get; }

	public ScreenshotViewModel(Screenshot value)
	{
		Value = value;
	}
}