using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Detector;

internal sealed class DetectorIterativeTagsUsageProvider : IterativeTagsUsageProvider<DetectorAsset>
{
	protected override bool IsInUse(DetectorAsset asset, Tag tag)
	{
		return asset.Items.Any(item => item.Tag == tag);
	}
}