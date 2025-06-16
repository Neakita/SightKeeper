using MemoryPack;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="DomainClassifierAsset"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableClassifierAsset : PackableAsset
{
	public required byte TagIndex { get; init; }
}