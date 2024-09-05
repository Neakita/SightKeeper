using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="DataSet"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableDataSet<PackableTag, PackableClassifierAsset>))]
internal abstract partial class PackableDataSet
{
	public string Name { get; }
	public string Description { get; }
	public ushort? GameId { get; }
	public PackableComposition? Composition { get; }
	public ImmutableArray<PackableScreenshot> Screenshots { get; }
	public abstract IReadOnlyCollection<PackableTag> Tags { get; }
	public abstract IReadOnlyCollection<PackableAsset> Assets { get; }

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
[MemoryPackable]
internal sealed partial class PackableDataSet<TTag, TAsset> : PackableDataSet
	where TTag : PackableTag
	where TAsset : PackableAsset
{
	public override ImmutableList<TTag> Tags { get; }
	public override ImmutableList<TAsset> Assets { get; }

	public PackableDataSet(
		string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ImmutableArray<PackableScreenshot> screenshots,
		ImmutableList<TTag> tags,
		ImmutableList<TAsset> assets)
		: base(name, description, gameId, composition, screenshots)
	{
		Tags = tags;
		Assets = assets;
	}
}