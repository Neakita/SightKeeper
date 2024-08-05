using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.Profiles;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
internal sealed partial class RawAppData
{
	public ImmutableArray<SerializableDataSet> DataSets { get; }
	public ImmutableArray<SerializableGame> Games { get; }
	public ImmutableArray<SerializableProfile> Profiles { get; }
	public SerializableApplicationSettings ApplicationSettings { get; }

	public RawAppData(ImmutableArray<SerializableGame> games,
		ImmutableArray<SerializableDataSet> dataSets,
		ImmutableArray<SerializableProfile> profiles,
		SerializableApplicationSettings applicationSettings)
	{
		DataSets = dataSets;
		Games = games;
		Profiles = profiles;
		ApplicationSettings = applicationSettings;
	}
}