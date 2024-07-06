using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary;

public sealed class AppData
{
	public HashSet<DetectorDataSet> DetectorDataSets { get; }
	public HashSet<Game> Games { get; }

	public AppData(HashSet<DetectorDataSet> detectorDataSets, HashSet<Game> games)
	{
		DetectorDataSets = detectorDataSets;
		Games = games;
	}
}