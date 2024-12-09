using FlakeId;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="Asset"/>
/// </summary>
internal abstract class PackableAsset
{
	public required AssetUsage Usage { get; init; }
	public required Id ScreenshotId { get; init; }
}