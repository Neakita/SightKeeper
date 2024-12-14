using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Screenshots;

public sealed class FixedTransparentImageComposition : ImageComposition
{
	public ImmutableArray<float> Opacities
	{
		get => _opacities;
		set
		{
			Guard.IsEqualTo(value.Sum(), 1);
			_opacities = value;
		}
	}

	public FixedTransparentImageComposition(
		TimeSpan maximumScreenshotsDelay,
		ImmutableArray<float> opacities)
		: base(maximumScreenshotsDelay)
	{
		Guard.IsGreaterThanOrEqualTo(opacities.Length, 2);
		Opacities = opacities;
	}

	private ImmutableArray<float> _opacities;
}