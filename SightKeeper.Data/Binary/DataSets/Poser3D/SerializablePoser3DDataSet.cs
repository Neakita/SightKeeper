using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Poser3D;

[MemoryPackable]
internal sealed partial class SerializablePoser3DDataSet : SerializableDataSet
{
	public ImmutableArray<SerializablePoser3DTag> Tags { get; }
	public ImmutableArray<SerializablePoser3DAsset> Assets { get; }
	public ImmutableArray<SerializablePoserWeights> Weights { get; }

	[MemoryPackConstructor]
	public SerializablePoser3DDataSet(string name, string description, ushort? gameId, ushort resolution, ushort? maxScreenshots, ImmutableArray<SerializableScreenshot> screenshots, ImmutableArray<SerializablePoser3DTag> tags, ImmutableArray<SerializablePoser3DAsset> assets, ImmutableArray<SerializablePoserWeights> weights) : base(name, description, gameId, resolution, maxScreenshots, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}

	public SerializablePoser3DDataSet(DataSet dataSet, ushort? gameId, ImmutableArray<SerializableScreenshot> screenshots, ImmutableArray<SerializablePoser3DTag> tags, ImmutableArray<SerializablePoser3DAsset> assets, ImmutableArray<SerializablePoserWeights> weights) : base(dataSet, gameId, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}
}