using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.DataSets.Weights.ImageCompositions;

public abstract class ImageComposition
{
	public TimeSpan MaximumScreenshotsDelay
	{
		get;
		set
		{
			Guard.IsGreaterThan(value, TimeSpan.Zero);
			field = value;
		}
	}

	protected ImageComposition(TimeSpan maximumScreenshotsDelay)
	{
		MaximumScreenshotsDelay = maximumScreenshotsDelay;
	}
}