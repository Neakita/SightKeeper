using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Screenshots;

public abstract class Composition
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

	protected Composition(TimeSpan maximumScreenshotsDelay)
	{
		MaximumScreenshotsDelay = maximumScreenshotsDelay;
	}

	private TimeSpan _maximumScreenshotsDelay;
}