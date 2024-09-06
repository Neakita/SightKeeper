using MemoryPack;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
internal sealed partial class PackableAppData
{
	public ApplicationSettings ApplicationSettings { get; }

	public PackableAppData(
		ApplicationSettings applicationSettings)
	{
		ApplicationSettings = applicationSettings;
	}
}