using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.DataSets.Detector;

[MemoryPackable]
internal partial class SerializableDetectorDataSet : SerializableDataSet
{
	public ImmutableArray<SerializableTag> Tags { get; }
	public ImmutableArray<SerializableDetectorAsset> Assets { get; }
	public ImmutableArray<SerializableWeightsWithTags> Weights { get; }

	[MemoryPackConstructor]
	public SerializableDetectorDataSet(
		string name,
		string description,
		ushort? gameId,
		ushort resolution,
		ushort? maxScreenshots,
		ImmutableArray<SerializableTag> tags,
		ImmutableArray<SerializableScreenshot> screenshots,
		ImmutableArray<SerializableDetectorAsset> assets,
		ImmutableArray<SerializableWeightsWithTags> weights)
		: base(name, description, gameId, resolution, maxScreenshots, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}

	public SerializableDetectorDataSet(
		DetectorDataSet dataSet,
		ushort? gameId,
		ImmutableArray<SerializableTag> tags,
		ImmutableArray<SerializableScreenshot> screenshots,
		ImmutableArray<SerializableDetectorAsset> assets,
		ImmutableArray<SerializableWeightsWithTags> weights)
		: base(dataSet, gameId, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}
}