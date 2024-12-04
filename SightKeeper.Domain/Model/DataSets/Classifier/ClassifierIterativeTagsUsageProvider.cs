using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

internal sealed class ClassifierIterativeTagsUsageProvider : IterativeTagsUsageProvider<ClassifierAsset>
{
	protected override bool IsInUse(ClassifierAsset asset, Tag tag)
	{
		return asset.Tag == tag;
	}
}