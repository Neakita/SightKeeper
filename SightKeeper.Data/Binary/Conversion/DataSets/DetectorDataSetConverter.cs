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
	public DetectorDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	public override PackableDetectorDataSet Convert(DataSet dataSet, ConversionSession session)
	{
		var packable = base.Convert(dataSet, session);
		packable.Tags = ConvertPlainTags(dataSet.TagsLibrary.Tags, session);
		packable.Assets = ConvertAssets(dataSet.AssetsLibrary.Assets, session);
		packable.Weights = ConvertPlainWeights(dataSet.WeightsLibrary.Weights, session).CastArray<PackablePlainWeights>();
		return packable;
	}

	private ImmutableArray<PackableItemsAsset<PackableDetectorItem>> ConvertAssets(IReadOnlyCollection<Asset> assets, ConversionSession session)
	{
		return assets.Cast<DetectorAsset>().Select(ConvertAsset).ToImmutableArray();
		PackableItemsAsset<PackableDetectorItem> ConvertAsset(DetectorAsset asset) => new(
			asset.Usage,
			ScreenshotsDataAccess.GetId(asset.Screenshot),
			asset.Items.Select(ConvertItem).ToImmutableArray());
		PackableDetectorItem ConvertItem(DetectorItem item) => new(session.TagsIds[item.Tag], item.Bounding);
	}
}