using System.Collections.Immutable;

namespace SightKeeper.Data.Binary.DataSets.Poser2D;

internal sealed class Poser2DDataSet : DataSet
{
	public ImmutableArray<Poser2DTag> Tags { get; }
	public ImmutableArray<Poser2DAsset> Assets { get; }
	public ImmutableArray<Poser.PoserWeights> Weights { get; }

	public Poser2DDataSet(string name, string description, ushort? gameId, ushort resolution, ushort? maxScreenshots, ImmutableArray<Screenshot> screenshots, ImmutableArray<Poser2DTag> tags, ImmutableArray<Poser2DAsset> assets, ImmutableArray<Poser.PoserWeights> weights) : base(name, description, gameId, resolution, maxScreenshots, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}

	public Poser2DDataSet(Domain.Model.DataSets.DataSet dataSet, ushort? gameId, ImmutableArray<Poser2DTag> tags,
		ImmutableArray<Screenshot> screenshots, ImmutableArray<Poser2DAsset> assets,
		ImmutableArray<Poser.PoserWeights> weights) : base(dataSet, gameId, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}
}