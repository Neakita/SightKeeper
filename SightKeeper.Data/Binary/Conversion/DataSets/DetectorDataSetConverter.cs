using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class DetectorDataSetConverter : DataSetConverter<PackableDetectorDataSet>
{
	public DetectorDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session) : base(screenshotsDataAccess, session)
	{
	}

	public override PackableDetectorDataSet Convert(DataSet dataSet)
	{
		var packable = base.Convert(dataSet);
		packable.Tags = ConvertPlainTags(dataSet.TagsLibrary.Tags);
		packable.Assets = ConvertAssets(dataSet.AssetsLibrary.Assets);
		packable.Weights = ConvertPlainWeights(dataSet.WeightsLibrary.Weights).CastArray<PackablePlainWeights>();
		return packable;
	}

	private ImmutableArray<PackableItemsAsset<PackableDetectorItem>> ConvertAssets(IReadOnlyCollection<Asset> assets)
	{
		return assets.Cast<DetectorAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableItemsAsset<PackableDetectorItem> ConvertAsset(DetectorAsset asset) => new(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			asset.Items.Select(ConvertItem).ToImmutableArray());
		PackableDetectorItem ConvertItem(DetectorItem item) => new(Session.TagsIds[item.Tag], item.Bounding);
	}
}