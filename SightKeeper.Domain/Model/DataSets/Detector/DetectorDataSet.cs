using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorDataSet : DataSet
{
	public override TagsLibrary<Tag> TagsLibrary { get; }
	public override AssetsLibrary<DetectorAsset> AssetsLibrary { get; }
	public override PlainWeightsLibrary WeightsLibrary { get; }

	public DetectorDataSet()
	{
		DetectorIterativeTagsUsageProvider tagsUsageProvider = new();
		TagsLibrary = new TagsLibrary<Tag>(PlainTagsFactory.Instance, tagsUsageProvider);
		DetectorAssetsFactory assetsFactory = new(TagsLibrary);
		AssetsLibrary = new AssetsLibrary<DetectorAsset>(assetsFactory);
		tagsUsageProvider.AssetsSource = AssetsLibrary.Assets.Values;
		WeightsLibrary = new PlainWeightsLibrary(1, TagsLibrary);
	}
}