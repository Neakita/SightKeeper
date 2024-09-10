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
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public ushort? GameId { get; set; }
	public PackableComposition? Composition { get; set; }
	public ushort? MaxScreenshotsWithoutAsset { get; set; }
	public ImmutableArray<PackableScreenshot> Screenshots { get; set; }

	public abstract ImmutableArray<PackableTag> GetTags();
	public abstract ImmutableArray<PackableAsset> GetAssets();
	public abstract ImmutableArray<PackableWeights> GetWeights();
}

/// <summary>
/// MemoryPackable version of <see cref="DataSet{TTag,TAsset}"/>
/// </summary>
internal abstract class PackableDataSet<TTag, TAsset, TWeights> : PackableDataSet
	where TTag : PackableTag
	where TAsset : PackableAsset
	where TWeights : PackableWeights
{
	public ImmutableArray<TTag> Tags { get; set; }
	public ImmutableArray<TAsset> Assets { get; set; }
	public ImmutableArray<TWeights> Weights { get; set; }

	public sealed override ImmutableArray<PackableTag> GetTags()
	{
		return ImmutableArray<PackableTag>.CastUp(Tags);
	}

	public sealed override ImmutableArray<PackableAsset> GetAssets()
	{
		return ImmutableArray<PackableAsset>.CastUp(Assets);
	}

	public sealed override ImmutableArray<PackableWeights> GetWeights()
	{
		return ImmutableArray<PackableWeights>.CastUp(Weights);
	}
}