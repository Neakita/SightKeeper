using System.Collections.Immutable;

namespace SightKeeper.Domain.DataSets.Weights.ImageCompositions;

public sealed class FixedTransparentImageComposition : ImageComposition
{
	public ImmutableArray<float> Opacities
	{
		get;
		set
		{
			if (value.Length < 2)
				throw new ArgumentException($"{nameof(Opacities)} value should contain 2 or more values, but was {value.Length}", nameof(value));
			ValidateOpacitiesSum(value, nameof(value));
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

	private static void ValidateOpacitiesSum(ImmutableArray<float> opacities, string paramName)
	{
		const double tolerance = 0.01;
		var opacitiesSum = opacities.Sum();
		var deviation = opacitiesSum - 1;
		if (Math.Abs(deviation) <= tolerance)
			return;
		var message = $"{nameof(Opacities)} value sum should be approximately equal to 1 with tolerance {tolerance}, " +
		              $"but was {opacitiesSum} with deviation {deviation}";
		throw new ArgumentException(message, paramName);
	}
}