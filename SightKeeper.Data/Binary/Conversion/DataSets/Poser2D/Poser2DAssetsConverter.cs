using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;

internal sealed class Poser2DAssetsConverter
{
	public Poser2DAssetsConverter(ConversionSession session, FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_session = session;
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public IEnumerable<PackableItemsAsset<PackablePoser2DItem>> ConvertAssets(IEnumerable<KeyValuePair<Screenshot, Poser2DAsset>> assets)
	{
		return assets.Select(ConvertAsset);
	}

	private readonly ConversionSession _session;
	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private PackableItemsAsset<PackablePoser2DItem> ConvertAsset(KeyValuePair<Screenshot, Poser2DAsset> assetPair)
	{
		var (screenshot, asset) = assetPair;
		return new PackableItemsAsset<PackablePoser2DItem>
		{
			Items = ConvertItems(asset.Items).ToImmutableArray(),
			Usage = asset.Usage,
			ScreenshotId = _screenshotsDataAccess.GetId(screenshot)
		};
	}

	private IEnumerable<PackablePoser2DItem> ConvertItems(IEnumerable<Poser2DItem> items)
	{
		return items.Select(ConvertItem);
	}

	private PackablePoser2DItem ConvertItem(Poser2DItem item) => new()
	{
		TagIndex = _session.TagsIndexes[item.Tag],
		Bounding = item.Bounding,
		KeyPoints = ConvertKeyPoints(item.KeyPoints).ToImmutableArray()
	};

	private IEnumerable<PackableKeyPoint> ConvertKeyPoints(IEnumerable<KeyPoint> keyPoints)
	{
		return keyPoints.Select(ConvertKeyPoint);
	}

	private PackableKeyPoint ConvertKeyPoint(KeyPoint keyPoint) => new()
	{
		TagIndex = _session.TagsIndexes[keyPoint.Tag],
		Position = keyPoint.Position
	};
}