using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary;

public sealed class AppData
{
	public HashSet<DetectorDataSet> DetectorDataSets { get; }
	public HashSet<Game> Games { get; }
	public HashSet<Profile> Profiles { get; }

	internal AppData(
		HashSet<DetectorDataSet> detectorDataSets,
		HashSet<Game> games,
		HashSet<Profile> profiles)
	{
		DetectorDataSets = detectorDataSets;
		Games = games;
		Profiles = profiles;
	}
}