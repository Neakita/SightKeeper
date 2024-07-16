using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Detector;

[MemoryPackable]
public partial class SerializableDetectorDataSet : SerializableDataSet
{
	public ImmutableArray<SerializableTag> Tags { get; }
	public ImmutableArray<SerializableDetectorAsset> Assets { get; }
	public ImmutableArray<SerializableDetectorWeights> Weights { get; }

	[MemoryPackConstructor]
	public SerializableDetectorDataSet(string name,
		string description,
		ushort? gameId,
		ushort resolution,
		ushort? maxScreenshots,
		ImmutableArray<SerializableTag> tags,
		ImmutableArray<SerializableScreenshot> screenshots,
		ImmutableArray<SerializableDetectorAsset> assets,
		ImmutableArray<SerializableDetectorWeights> weights)
		: base(name, description, gameId, resolution, maxScreenshots, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}

	public SerializableDetectorDataSet(DataSet dataSet,
		ushort? gameId,
		ImmutableArray<SerializableTag> tags,
		ImmutableArray<SerializableScreenshot> screenshots,
		ImmutableArray<SerializableDetectorAsset> assets,
		ImmutableArray<SerializableDetectorWeights> weights)
		: base(dataSet, gameId, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}
}