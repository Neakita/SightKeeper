using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SightKeeper.Domain.Model.Profiles;

public sealed class PreemptionStabilizationSettings : ObservableObject
{
	public byte BufferSize { get; private set; }
	public PreemptionStabilizationMethod Method { get; private set; }

	public PreemptionStabilizationSettings(byte bufferSize, PreemptionStabilizationMethod method)
	{
		Guard.IsGreaterThanOrEqualTo((int)bufferSize, 2);
		BufferSize = bufferSize;
		Method = method;
	}

	private PreemptionStabilizationSettings()
	{
	}
}