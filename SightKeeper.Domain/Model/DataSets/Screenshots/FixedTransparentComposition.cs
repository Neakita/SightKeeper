using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Screenshots;

public sealed class FixedTransparentComposition : Composition
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

	public FixedTransparentComposition(
		TimeSpan maximumScreenshotsDelay,
		ImmutableArray<float> opacities)
		: base(maximumScreenshotsDelay)
	{
		Guard.IsGreaterThanOrEqualTo(opacities.Length, 2);
		Opacities = opacities;
	}

	private ImmutableArray<float> _opacities;
}