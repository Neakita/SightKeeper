using FlakeId;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="Asset"/>
/// </summary>
internal abstract class PackableAsset
{
	public required Id ImageId { get; init; }
}