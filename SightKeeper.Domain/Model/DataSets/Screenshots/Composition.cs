using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Screenshots;

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
		_maximumScreenshotsDelay = maximumScreenshotsDelay;
	}

	private TimeSpan _maximumScreenshotsDelay;
}