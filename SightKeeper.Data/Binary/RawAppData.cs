using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
public sealed partial class RawAppData
{
	public ImmutableArray<SerializableDetectorDataSet> DetectorDataSets { get; }
	public ImmutableArray<SerializableGame> Games { get; }

	public RawAppData(ImmutableArray<SerializableDetectorDataSet> detectorDataSets, ImmutableArray<SerializableGame> games)
	{
		DetectorDataSets = detectorDataSets;
		Games = games;
	}
}