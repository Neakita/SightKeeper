using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.Profiles;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
internal sealed partial class RawAppData
{
	public ImmutableArray<DataSet> DataSets { get; }
	public ImmutableArray<Game> Games { get; }
	public ImmutableArray<Profile> Profiles { get; }
	public ApplicationSettings ApplicationSettings { get; }

	public RawAppData(ImmutableArray<Game> games,
		ImmutableArray<DataSet> dataSets,
		ImmutableArray<Profile> profiles,
		ApplicationSettings applicationSettings)
	{
		DataSets = dataSets;
		Games = games;
		Profiles = profiles;
		ApplicationSettings = applicationSettings;
	}
}