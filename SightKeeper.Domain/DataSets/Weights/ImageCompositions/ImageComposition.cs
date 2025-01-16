namespace SightKeeper.Domain.DataSets.Weights.ImageCompositions;

public abstract class ImageComposition
{
	public TimeSpan MaximumScreenshotsDelay
	{
		get;
		set
		{
			if (value <= TimeSpan.Zero)
				throw new ArgumentException($"{nameof(MaximumScreenshotsDelay)} value should be greater than zero, but was {value}", nameof(value));
			field = value;
		}
	}

	protected ImageComposition(TimeSpan maximumScreenshotsDelay)
	{
		MaximumScreenshotsDelay = maximumScreenshotsDelay;
	}
}