using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Classifier;

internal sealed class ClassifierIterativeTagsUsageProvider : IterativeTagsUsageProvider<ClassifierAsset>
{
	protected override bool IsInUse(ClassifierAsset asset, Tag tag)
	{
		return asset.Tag == tag;
	}
}