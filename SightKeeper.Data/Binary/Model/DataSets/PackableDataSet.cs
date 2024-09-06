using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="DataSet"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableClassifierDataSet))]
[MemoryPackUnion(1, typeof(PackableDetectorDataSet))]
[MemoryPackUnion(2, typeof(PackablePoser2DDataSet))]
[MemoryPackUnion(3, typeof(PackablePoser3DDataSet))]
internal abstract partial class PackableDataSet
{
	public string Name { get; }
	public string Description { get; }
	public ushort? GameId { get; }
	public PackableComposition? Composition { get; }
	public ImmutableArray<PackableScreenshot> Screenshots { get; }
	public abstract IReadOnlyCollection<PackableTag> Tags { get; }
	public abstract IReadOnlyCollection<PackableAsset> Assets { get; }
	public abstract IReadOnlyCollection<PackableWeights> Weights { get; }

	public PackableDataSet(
		string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ImmutableArray<PackableScreenshot> screenshots)
	{
		Name = name;
		Description = description;
		GameId = gameId;
		Composition = composition;
		Screenshots = screenshots;
	}
}

/// <summary>
/// MemoryPackable version of <see cref="DataSet{TTag,TAsset}"/>
/// </summary>
internal abstract class PackableDataSet<TTag, TAsset, TWeights> : PackableDataSet
	where TTag : PackableTag
	where TAsset : PackableAsset
	where TWeights : PackableWeights
{
	public override ImmutableList<TTag> Tags { get; }
	public override ImmutableList<TAsset> Assets { get; }
	public override ImmutableList<TWeights> Weights { get; }

	public PackableDataSet(
		string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ImmutableArray<PackableScreenshot> screenshots,
		IEnumerable<PackableTag> tags,
		IEnumerable<PackableAsset> assets,
		IEnumerable<PackableWeights> weights)
		: base(name, description, gameId, composition, screenshots)
	{
		Tags = tags.Cast<TTag>().ToImmutableList();
		Assets = assets.Cast<TAsset>().ToImmutableList();
		Weights = weights.Cast<TWeights>().ToImmutableList();
	}
}