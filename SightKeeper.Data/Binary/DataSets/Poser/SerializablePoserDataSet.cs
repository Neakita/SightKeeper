using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Poser;

internal sealed class SerializablePoserDataSet : SerializableDataSet
{
	public ImmutableArray<SerializablePoserTag> Tags { get; }
	public ImmutableArray<SerializablePoserAsset> Assets { get; }
	public ImmutableArray<SerializablePoserWeights> Weights { get; }

	public SerializablePoserDataSet(string name, string description, ushort? gameId, ushort resolution, ushort? maxScreenshots, ImmutableArray<SerializableScreenshot> screenshots, ImmutableArray<SerializablePoserTag> tags, ImmutableArray<SerializablePoserAsset> assets, ImmutableArray<SerializablePoserWeights> weights) : base(name, description, gameId, resolution, maxScreenshots, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}

	public SerializablePoserDataSet(DataSet dataSet, ushort? gameId, ImmutableArray<SerializablePoserTag> tags,
		ImmutableArray<SerializableScreenshot> screenshots, ImmutableArray<SerializablePoserAsset> assets,
		ImmutableArray<SerializablePoserWeights> weights) : base(dataSet, gameId, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}
}