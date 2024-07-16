using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary;

public sealed class AppData
{
	public HashSet<DataSet> DataSets { get; }
	public HashSet<Game> Games { get; }
	public HashSet<Profile> Profiles { get; }

	internal AppData(
		HashSet<Game> games,
		HashSet<DataSet> dataSets,
		HashSet<Profile> profiles)
	{
		DataSets = dataSets;
		Games = games;
		Profiles = profiles;
	}
}