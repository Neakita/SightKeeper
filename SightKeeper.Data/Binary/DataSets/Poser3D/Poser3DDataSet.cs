using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.DataSets.Poser;

namespace SightKeeper.Data.Binary.DataSets.Poser3D;

[MemoryPackable]
internal sealed partial class Poser3DDataSet : DataSet
{
	public ImmutableArray<Poser3DTag> Tags { get; }
	public ImmutableArray<Poser3DAsset> Assets { get; }
	public ImmutableArray<PoserWeights> Weights { get; }

	[MemoryPackConstructor]
	public Poser3DDataSet(string name, string description, ushort? gameId, ushort resolution, ushort? maxScreenshots, ImmutableArray<Screenshot> screenshots, ImmutableArray<Poser3DTag> tags, ImmutableArray<Poser3DAsset> assets, ImmutableArray<PoserWeights> weights) : base(name, description, gameId, resolution, maxScreenshots, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}

	public Poser3DDataSet(Domain.Model.DataSets.DataSet dataSet, ushort? gameId, ImmutableArray<Screenshot> screenshots, ImmutableArray<Poser3DTag> tags, ImmutableArray<Poser3DAsset> assets, ImmutableArray<PoserWeights> weights) : base(dataSet, gameId, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}
}