using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Classifier;

[MemoryPackable]
public sealed partial class SerializableClassifierDataSet : SerializableDataSet
{
	public ImmutableArray<SerializableTag> Tags { get; }
	public ImmutableArray<SerializableClassifierAsset> Assets { get; }
	public ImmutableArray<SerializableWeightsWithTags> Weights { get; }

	[MemoryPackConstructor]
	public SerializableClassifierDataSet(string name,
		string description,
		ushort? gameId,
		ushort resolution,
		ushort? maxScreenshots,
		ImmutableArray<SerializableTag> tags,
		ImmutableArray<SerializableScreenshot> screenshots,
		ImmutableArray<SerializableClassifierAsset> assets,
		ImmutableArray<SerializableWeightsWithTags> weights)
		: base(name, description, gameId, resolution, maxScreenshots, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}

	public SerializableClassifierDataSet(DataSet dataSet,
		ushort? gameId,
		ImmutableArray<SerializableTag> tags,
		ImmutableArray<SerializableScreenshot> screenshots,
		ImmutableArray<SerializableClassifierAsset> assets,
		ImmutableArray<SerializableWeightsWithTags> weights)
		: base(dataSet, gameId, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}
}