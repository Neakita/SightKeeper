using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Screenshots;

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

	public float MinimumOpacity
	{
		get => _minimumOpacity;
		set
		{
			Guard.IsInRange(value, 0, 1);
			_minimumOpacity = value;
		}
	}

	public FloatingTransparentComposition(TimeSpan maximumScreenshotsDelay, TimeSpan seriesDuration, float minimumOpacity) : base(maximumScreenshotsDelay)
	{
		SeriesDuration = seriesDuration;
		MinimumOpacity = minimumOpacity;
	}

	private TimeSpan _seriesDuration;
	private float _minimumOpacity;
}