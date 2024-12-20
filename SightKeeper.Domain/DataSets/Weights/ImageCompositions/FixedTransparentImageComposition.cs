using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.DataSets.Weights.ImageCompositions;

public sealed class FixedTransparentImageComposition : ImageComposition
{
	public ImmutableArray<float> Opacities
	{
		get;
		set
		{
			Guard.IsGreaterThanOrEqualTo(value.Length, 2);
			Guard.IsEqualTo(value.Sum(), 1);
			field = value;
		}
	}

	public FixedTransparentImageComposition(
		TimeSpan maximumScreenshotsDelay,
		ImmutableArray<float> opacities)
		: base(maximumScreenshotsDelay)
	{
		Opacities = opacities;
	}
}