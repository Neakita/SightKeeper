using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Screenshots;

public sealed class FloatingTransparentComposition : Composition
{
	public TimeSpan SeriesDuration
	{
		get => _seriesDuration;
		set
		{
			Guard.IsGreaterThan(value, TimeSpan.Zero);
			_seriesDuration = value;
		}
	}

	public float PrimaryOpacity
	{
		get => _primaryOpacity;
		set
		{
			Guard.IsInRange(value, 0, 1);
			_primaryOpacity = value;
		}
	}

	public float MinimumOpacity
	{
		get => _minimumOpacity;
		set
		{
			Guard.IsInRange(value, 0, 1);
			_minimumOpacity = value;
		}
	}

	public FloatingTransparentComposition(
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

	private TimeSpan _seriesDuration;
	private float _minimumOpacity;
	private float _primaryOpacity;
}