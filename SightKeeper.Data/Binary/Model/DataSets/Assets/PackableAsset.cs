using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="Asset"/>
/// </summary>
internal abstract class PackableAsset
{
	public AssetUsage Usage { get; }
	public uint ScreenshotId { get; }

	protected PackableAsset(AssetUsage usage, uint screenshotId)
	{
		Usage = usage;
		ScreenshotId = screenshotId;
	}
}