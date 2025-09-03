using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Application.Training.Data;

public static class TrainData
{
	public static TrainData<ReadOnlyAsset> Create(DataSet dataSet)
	{
		var tags = dataSet.TagsLibrary.Tags;
		return dataSet switch
		{
			ClassifierDataSet classifierDataSet => throw new NotImplementedException(),
			DetectorDataSet detectorDataSet => new TrainDataValue<ReadOnlyItemsAsset<ReadOnlyAssetItem>>(tags, detectorDataSet.AssetsLibrary.Assets.Select(ConvertAsset)),
			PoserDataSet poserDataSet => throw new NotImplementedException(),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};

		ReadOnlyItemsAsset<ReadOnlyAssetItem> ConvertAsset(ItemsAsset<DetectorItem> asset)
		{
			var items = asset.Items.Select(ConvertItem).ToList();
			return new ReadOnlyItemsAssetValue<ReadOnlyAssetItem>(asset.Image, asset.Usage, items);
		}

		ReadOnlyAssetItem ConvertItem(DetectorItem item)
		{
			return new AssetItemDataValue(item.Tag, item.Bounding);
		}
	}
}