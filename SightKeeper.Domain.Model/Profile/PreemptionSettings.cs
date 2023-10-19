using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model;

public sealed class PreemptionSettings
{
	public float HorizontalFactor { get; private set; }
	public float VerticalFactor { get; private set; }
	public PreemptionStabilizationSettings StabilizationSettings { get; private set; }

	public PreemptionSettings(float horizontalFactor, float verticalFactor, byte bufferSize, PreemptionStabilizationMethod method)
	{
		Guard.IsGreaterThan(horizontalFactor, 0);
		Guard.IsGreaterThan(verticalFactor, 0);
		HorizontalFactor = horizontalFactor;
		VerticalFactor = verticalFactor;
		StabilizationSettings = new PreemptionStabilizationSettings(bufferSize, method);
	}

	private PreemptionSettings()
	{
		StabilizationSettings = null!;
	}
}