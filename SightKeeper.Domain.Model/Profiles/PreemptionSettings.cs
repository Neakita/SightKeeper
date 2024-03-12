using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Profiles;

public sealed class PreemptionSettings
{
	public Vector2<float> Factor { get; }
	public StabilizationSettings? StabilizationSettings { get; }

	public PreemptionSettings(Vector2<float> factor)
	{
		Guard.IsGreaterThanOrEqualTo(factor.X, 0);
		Guard.IsGreaterThanOrEqualTo(factor.Y, 0);
		Factor = factor;
	}

	public PreemptionSettings(Vector2<float> factor, StabilizationSettings stabilizationSettings) : this(factor)
	{
		StabilizationSettings = stabilizationSettings;
	}
}