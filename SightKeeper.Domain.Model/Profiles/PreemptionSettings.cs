using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SightKeeper.Domain.Model.Profiles;

public sealed class PreemptionSettings : ObservableObject
{
	public float HorizontalFactor { get; private set; }
	public float VerticalFactor { get; private set; }
	public PreemptionStabilizationSettings? StabilizationSettings { get; private set; }

	public PreemptionSettings(float horizontalFactor, float verticalFactor)
	{
		Guard.IsGreaterThanOrEqualTo(horizontalFactor, 0);
		Guard.IsGreaterThanOrEqualTo(verticalFactor, 0);
		HorizontalFactor = horizontalFactor;
		VerticalFactor = verticalFactor;
	}

	public PreemptionSettings(float horizontalFactor, float verticalFactor, byte bufferSize, PreemptionStabilizationMethod method) : this(horizontalFactor, verticalFactor)
	{
		StabilizationSettings = new PreemptionStabilizationSettings(bufferSize, method);
	}

	private PreemptionSettings()
	{
		StabilizationSettings = null!;
	}
}