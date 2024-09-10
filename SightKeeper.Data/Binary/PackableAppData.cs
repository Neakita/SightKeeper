using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.Profiles;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
internal sealed partial class PackableAppData
{
	public ImmutableArray<PackableGame> Games { get; }
	public ImmutableArray<PackableDataSet> DataSets { get; }
	public ImmutableArray<PackableProfile> Profiles { get; }
	public ApplicationSettings ApplicationSettings { get; }

	public PackableAppData(
		ImmutableArray<PackableGame> games,
		ImmutableArray<PackableDataSet> dataSets,
		ImmutableArray<PackableProfile> profiles,
		ApplicationSettings applicationSettings)
	{
		Games = games;
		DataSets = dataSets;
		Profiles = profiles;
		ApplicationSettings = applicationSettings;
	}
}