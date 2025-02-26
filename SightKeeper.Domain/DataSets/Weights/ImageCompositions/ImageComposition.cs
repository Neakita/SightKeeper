namespace SightKeeper.Domain.DataSets.Weights.ImageCompositions;

public abstract class ImageComposition
{
	public TimeSpan MaximumDelay
	{
		get;
		set
		{
			if (value <= TimeSpan.Zero)
				throw new ArgumentException($"{nameof(MaximumDelay)} value should be greater than zero, but was {value}", nameof(value));
			field = value;
		}
	}

	protected ImageComposition(TimeSpan maximumDelay)
	{
		MaximumDelay = maximumDelay;
	}
}