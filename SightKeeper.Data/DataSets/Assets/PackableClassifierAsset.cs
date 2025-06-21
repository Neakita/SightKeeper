using MemoryPack;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="ClassifierAsset"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableClassifierAsset : PackableAsset
{
	public required byte TagIndex { get; init; }
}