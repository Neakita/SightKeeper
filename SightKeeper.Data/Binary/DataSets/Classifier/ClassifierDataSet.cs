using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary.DataSets.Classifier;

[MemoryPackable]
internal sealed partial class ClassifierDataSet : DataSet
{
	public ImmutableArray<Tag> Tags { get; }
	public ImmutableArray<ClassifierAsset> Assets { get; }
	public ImmutableArray<WeightsWithTags> Weights { get; }

	[MemoryPackConstructor]
	public ClassifierDataSet(
		string name,
		string description,
		ushort? gameId,
		ushort resolution,
		ushort? maxScreenshots,
		ImmutableArray<Tag> tags,
		ImmutableArray<Screenshot> screenshots,
		ImmutableArray<ClassifierAsset> assets,
		ImmutableArray<WeightsWithTags> weights)
		: base(name, description, gameId, resolution, maxScreenshots, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}

	public ClassifierDataSet(
		Domain.Model.DataSets.Classifier.ClassifierDataSet dataSet,
		ushort? gameId,
		ImmutableArray<Tag> tags,
		ImmutableArray<Screenshot> screenshots,
		ImmutableArray<ClassifierAsset> assets,
		ImmutableArray<WeightsWithTags> weights)
		: base(dataSet, gameId, screenshots)
	{
		Tags = tags;
		Assets = assets;
		Weights = weights;
	}
}