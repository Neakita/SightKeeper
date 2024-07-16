using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Detector;

[MemoryPackable]
public partial class SerializableDetectorDataSet : SerializableDataSet
{
	public IReadOnlyCollection<SerializableTag> Tags { get; }
	public IReadOnlyCollection<SerializableDetectorAsset> Assets { get; }
	public IReadOnlyCollection<SerializableDetectorWeights> Weights { get; }

	[MemoryPackConstructor]
	public SerializableDetectorDataSet(
		string name,
		string description,
		ushort? gameId,
		ushort resolution,
		ushort? maxScreenshots,
		IReadOnlyCollection<SerializableTag> tags,
		IReadOnlyCollection<SerializableDetectorAsset> assets,
		IReadOnlyCollection<SerializableDetectorWeights> weights,
		IReadOnlyCollection<SerializableScreenshot> screenshots)
		: base(name, description, gameId, resolution, maxScreenshots, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}

	public SerializableDetectorDataSet(
		DataSet dataSet,
		ushort? gameId,
		IReadOnlyCollection<SerializableScreenshot> screenshots,
		IReadOnlyCollection<SerializableTag> tags,
		IReadOnlyCollection<SerializableDetectorAsset> assets,
		IReadOnlyCollection<SerializableDetectorWeights> weights)
		: base(dataSet, gameId, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}
}