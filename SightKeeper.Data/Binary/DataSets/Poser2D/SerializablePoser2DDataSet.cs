using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Poser2D;

internal sealed class SerializablePoser2DDataSet : SerializableDataSet
{
	public ImmutableArray<SerializablePoser2DTag> Tags { get; }
	public ImmutableArray<SerializablePoser2DAsset> Assets { get; }
	public ImmutableArray<Poser.SerializablePoserWeights> Weights { get; }

	public SerializablePoser2DDataSet(string name, string description, ushort? gameId, ushort resolution, ushort? maxScreenshots, ImmutableArray<SerializableScreenshot> screenshots, ImmutableArray<SerializablePoser2DTag> tags, ImmutableArray<SerializablePoser2DAsset> assets, ImmutableArray<Poser.SerializablePoserWeights> weights) : base(name, description, gameId, resolution, maxScreenshots, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}

	public SerializablePoser2DDataSet(DataSet dataSet, ushort? gameId, ImmutableArray<SerializablePoser2DTag> tags,
		ImmutableArray<SerializableScreenshot> screenshots, ImmutableArray<SerializablePoser2DAsset> assets,
		ImmutableArray<Poser.SerializablePoserWeights> weights) : base(dataSet, gameId, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}
}