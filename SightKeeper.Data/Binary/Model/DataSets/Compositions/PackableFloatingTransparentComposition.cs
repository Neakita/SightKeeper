using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Model.DataSets.Compositions;

/// <summary>
/// MemoryPackable version of <see cref="FloatingTransparentComposition"/>
/// </summary>
internal sealed class PackableFloatingTransparentComposition : PackableComposition
{
	public TimeSpan SeriesDuration { get; }
	public float PrimaryOpacity { get; }
	public float MinimumOpacity { get; }

	public PackableFloatingTransparentComposition(
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
}