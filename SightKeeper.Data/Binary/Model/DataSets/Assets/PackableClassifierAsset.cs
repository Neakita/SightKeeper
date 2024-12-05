using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="ClassifierAsset"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableClassifierAsset : PackableAsset
{
	public required byte TagId { get; init; }
}