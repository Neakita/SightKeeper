using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Application.Training.Data;

public static class TrainData
{
	public static TrainData<AssetData> Create(DataSet dataSet)
	{
		var tagsData = dataSet.TagsLibrary.Tags.Select(tag => new TagDataValue(tag.Name)).ToList();
		var tagsDataLookup = dataSet.TagsLibrary.Tags.Zip(tagsData).ToDictionary(tuple => tuple.First, tuple => tuple.Second);
		return dataSet switch
		{
			ClassifierDataSet classifierDataSet => throw new NotImplementedException(),
			DetectorDataSet detectorDataSet => new TrainDataValue<ItemsAssetData<AssetItemData>>(tagsData, detectorDataSet.AssetsLibrary.Assets.Select(ConvertAsset)),
			Poser2DDataSet poser2DDataSet => throw new NotImplementedException(),
			Poser3DDataSet poser3DDataSet => throw new NotImplementedException(),
			_ => throw new ArgumentOutOfRangeException(nameof(dataSet))
		};

		ItemsAssetData<AssetItemData> ConvertAsset(ItemsAsset<DetectorItem> asset)
		{
			var items = asset.Items.Select(ConvertItem).ToList();
			return new ItemsAssetDataValue<AssetItemData>(asset.Image, asset.Usage, items);
		}

		AssetItemData ConvertItem(DetectorItem item)
		{
			var tag = tagsDataLookup[item.Tag];
			return new AssetItemDataValue(tag, item.Bounding);
		}
	}
}