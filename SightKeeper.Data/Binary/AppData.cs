using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary;

public sealed class AppData
{
	public HashSet<DataSet> DataSets { get; } = new();
	public HashSet<Game> Games { get; } = new();
}