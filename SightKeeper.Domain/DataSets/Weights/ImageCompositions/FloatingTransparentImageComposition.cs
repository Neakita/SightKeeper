namespace SightKeeper.Domain.DataSets.Weights.ImageCompositions;

public sealed class FloatingTransparentImageComposition : ImageComposition
{
	public TimeSpan SeriesDuration
	{
		get;
		set
		{
			if (value <= TimeSpan.Zero)
				throw new ArgumentException($"{nameof(SeriesDuration)} should be greater than zero, but was {value}",
					nameof(value));
			field = value;
		}
	}

	public float PrimaryOpacity
	{
		get;
		set
		{
			ValidateValueIsNormalized(value, nameof(PrimaryOpacity), nameof(value));
			field = value;
		}
	}

	public float MinimumOpacity
	{
		get;
		set
		{
			ValidateValueIsNormalized(value, nameof(MinimumOpacity), nameof(value));
			field = value;
		}
	}

	public FloatingTransparentImageComposition(
		TimeSpan maximumScreenshotsDelay,
		TimeSpan seriesDuration,
		float primaryOpacity,
		float minimumOpacity)
		: base(maximumScreenshotsDelay)
	{
		SeriesDuration = seriesDuration;
		PrimaryOpacity = primaryOpacity;
		MinimumOpacity = minimumOpacity;
	}

	private static void ValidateValueIsNormalized(float value, string propertyName, string paramName)
	{
		if (value <= 0)
			throw new ArgumentException($"{propertyName} value should be greater than 0, but was {value}", paramName);
		if (value >= 1)
			throw new ArgumentException($"{propertyName} value should be lesser than 1, buf was {value}", paramName);
	}
}