using System.Collections.Immutable;
using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Data.Conversion.DataSets.Detector;

internal sealed class DetectorAssetsConverter
{
	public DetectorAssetsConverter(FileSystemImageDataAccess imageDataAccess, ConversionSession session)
	{
		_imageDataAccess = imageDataAccess;
		_session = session;
	}

	public IEnumerable<PackableItemsAsset<PackableDetectorItem>> ConvertAssets(IEnumerable<DetectorAsset> assets)
	{
		return assets.Select(ConvertAsset);
	}

	private readonly FileSystemImageDataAccess _imageDataAccess;
	private readonly ConversionSession _session;

	private PackableItemsAsset<PackableDetectorItem> ConvertAsset(DetectorAsset asset)
	{
		var image = asset.Image;
		return new PackableItemsAsset<PackableDetectorItem>
		{
			Usage = asset.Usage,
			ImageId = _imageDataAccess.GetId(image),
			Items = ConvertItems(asset.Items).ToImmutableArray()
		};
	}

	private IEnumerable<PackableDetectorItem> ConvertItems(IEnumerable<DetectorItem> items)
	{
		return items.Select(ConvertItem);
	}

	private PackableDetectorItem ConvertItem(DetectorItem item)
	{
		var tagId = _session.TagsIndexes[item.Tag];
		return new PackableDetectorItem(tagId, item.Bounding);
	}
}