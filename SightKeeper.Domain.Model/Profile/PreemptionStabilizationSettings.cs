using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model;

public sealed class PreemptionStabilizationSettings
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