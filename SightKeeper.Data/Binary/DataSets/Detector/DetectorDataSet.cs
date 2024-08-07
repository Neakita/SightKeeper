using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary.DataSets.Detector;

[MemoryPackable]
internal partial class DetectorDataSet : DataSet
{
	public ImmutableArray<Tag> Tags { get; }
	public ImmutableArray<DetectorAsset> Assets { get; }
	public ImmutableArray<WeightsWithTags> Weights { get; }

	[MemoryPackConstructor]
	public DetectorDataSet(
		string name,
		string description,
		ushort? gameId,
		ushort resolution,
		ushort? maxScreenshots,
		ImmutableArray<Tag> tags,
		ImmutableArray<Screenshot> screenshots,
		ImmutableArray<DetectorAsset> assets,
		ImmutableArray<WeightsWithTags> weights)
		: base(name, description, gameId, resolution, maxScreenshots, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}

	public DetectorDataSet(
		Domain.Model.DataSets.Detector.DetectorDataSet dataSet,
		ushort? gameId,
		ImmutableArray<Tag> tags,
		ImmutableArray<Screenshot> screenshots,
		ImmutableArray<DetectorAsset> assets,
		ImmutableArray<WeightsWithTags> weights)
		: base(dataSet, gameId, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}
}