using MemoryPack;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
internal sealed partial class RawAppData
{
	public ApplicationSettings ApplicationSettings { get; }

	public RawAppData(
		ApplicationSettings applicationSettings)
	{
		ApplicationSettings = applicationSettings;
	}
}