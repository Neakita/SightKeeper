using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Screenshots;

public abstract class ImageComposition
{	
	public TimeSpan MaximumScreenshotsDelay
	{
		get => _maximumScreenshotsDelay;
		set
		{
			Guard.IsGreaterThan(value, TimeSpan.Zero);
			_maximumScreenshotsDelay = value;
		}
	}

	protected ImageComposition(TimeSpan maximumScreenshotsDelay)
	{
		MaximumScreenshotsDelay = maximumScreenshotsDelay;
	}

	private TimeSpan _maximumScreenshotsDelay;
}