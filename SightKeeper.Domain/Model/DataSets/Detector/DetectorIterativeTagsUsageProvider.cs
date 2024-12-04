using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Detector;

internal sealed class DetectorIterativeTagsUsageProvider : IterativeTagsUsageProvider<DetectorAsset>
{
	protected override bool IsInUse(DetectorAsset asset, Tag tag)
	{
		return asset.Items.Any(item => item.Tag == tag);
	}
}