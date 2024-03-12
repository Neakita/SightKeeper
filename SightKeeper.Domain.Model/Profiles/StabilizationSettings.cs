using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Profiles;

public sealed class StabilizationSettings
{
	public byte BufferSize { get; }
	public StabilizationMethod Method { get; }

	public StabilizationSettings(byte bufferSize, StabilizationMethod method)
	{
		Guard.IsGreaterThanOrEqualTo((int)bufferSize, 2);
		BufferSize = bufferSize;
		Method = method;
	}
}