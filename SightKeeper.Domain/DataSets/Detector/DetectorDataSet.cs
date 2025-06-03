using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.DataSets.Detector;

public sealed class DetectorDataSet : DataSet
{
	public override TagsLibrary<Tag> TagsLibrary { get; }
	public override AssetsLibrary<DetectorAsset> AssetsLibrary { get; }
	public override WeightsLibrary WeightsLibrary { get; }

	public DetectorDataSet()
	{
		TagsLibrary = new TagsLibrary<Tag>(PlainTagsFactory.Instance)
		{
			DataSet = this
		};
		DetectorAssetsFactory assetsFactory = new(TagsLibrary);
		AssetsLibrary = new AssetsLibrary<DetectorAsset>(assetsFactory);
		WeightsLibrary = new WeightsLibrary(TagsLibrary);
	}
}