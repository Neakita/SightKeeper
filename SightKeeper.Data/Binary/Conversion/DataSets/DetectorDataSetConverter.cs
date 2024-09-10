using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class DetectorDataSetConverter : DataSetConverter<PackableTag, PackableItemsAsset<PackableDetectorItem>, PackablePlainWeights, PackableDetectorDataSet>
{
	public DetectorDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session) : base(screenshotsDataAccess, session)
	{
	}

	protected override ImmutableArray<PackableTag> ConvertTags(IReadOnlyCollection<Tag> tags)
	{
		return ConvertPlainTags(tags);
	}

	protected override ImmutableArray<PackableItemsAsset<PackableDetectorItem>> ConvertAssets(IReadOnlyCollection<Asset> assets)
	{
		return assets.Cast<DetectorAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableItemsAsset<PackableDetectorItem> ConvertAsset(DetectorAsset asset) => new(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			asset.Items.Select(ConvertItem).ToImmutableArray());
		PackableDetectorItem ConvertItem(DetectorItem item) => new(Session.TagsIds[item.Tag], item.Bounding);
	}

	protected override ImmutableArray<PackablePlainWeights> ConvertWeights(IReadOnlyCollection<Weights> weights)
	{
		return ConvertPlainWeights(weights);
	}
}