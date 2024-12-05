using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Detector;

internal sealed class DetectorAssetsConverter
{
	public DetectorAssetsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
		_session = session;
	}

	public IEnumerable<PackableItemsAsset<PackableDetectorItem>> ConvertAssets(IEnumerable<KeyValuePair<Screenshot, DetectorAsset>> assets)
	{
		return assets.Select(ConvertAsset);
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
	private readonly ConversionSession _session;

	private PackableItemsAsset<PackableDetectorItem> ConvertAsset(KeyValuePair<Screenshot, DetectorAsset> assetPair)
	{
		var (screenshot, asset) = assetPair;
		return new PackableItemsAsset<PackableDetectorItem>
		{
			Usage = asset.Usage,
			ScreenshotId = _screenshotsDataAccess.GetId(screenshot),
			Items = ConvertItems(asset.Items).ToImmutableArray()
		};
	}

	private IEnumerable<PackableDetectorItem> ConvertItems(IEnumerable<DetectorItem> items)
	{
		return items.Select(ConvertItem);
	}

	private PackableDetectorItem ConvertItem(DetectorItem item)
	{
		var tagId = _session.TagsIds[item.Tag];
		return new PackableDetectorItem(tagId, item.Bounding);
	}
}