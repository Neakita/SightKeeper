using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
internal sealed partial class PackableAppData
{
	public required ImmutableArray<PackableDataSet> DataSets { get; init; }
	public required ApplicationSettings ApplicationSettings { get; init; }
}