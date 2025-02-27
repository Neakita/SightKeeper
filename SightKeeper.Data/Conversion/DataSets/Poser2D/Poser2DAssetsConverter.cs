using System.Collections.Immutable;
using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Conversion.DataSets.Poser2D;

internal sealed class Poser2DAssetsConverter
{
	public Poser2DAssetsConverter(ConversionSession session, FileSystemImageDataAccess imageDataAccess)
	{
		_session = session;
		_imageDataAccess = imageDataAccess;
	}

	public IEnumerable<PackableItemsAsset<PackablePoser2DItem>> ConvertAssets(IEnumerable<KeyValuePair<Image, Poser2DAsset>> assets)
	{
		return assets.Select(ConvertAsset);
	}

	private readonly ConversionSession _session;
	private readonly FileSystemImageDataAccess _imageDataAccess;

	private PackableItemsAsset<PackablePoser2DItem> ConvertAsset(KeyValuePair<Image, Poser2DAsset> assetPair)
	{
		var (image, asset) = assetPair;
		return new PackableItemsAsset<PackablePoser2DItem>
		{
			Items = ConvertItems(asset.Items).ToImmutableArray(),
			Usage = asset.Usage,
			ImageId = _imageDataAccess.GetId(image)
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