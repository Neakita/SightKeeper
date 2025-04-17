using System.Collections.Immutable;
using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Conversion.DataSets.Poser3D;

internal sealed class Poser3DAssetsConverter
{
	public Poser3DAssetsConverter(ConversionSession session, FileSystemImageDataAccess imageDataAccess)
	{
		_session = session;
		_imageDataAccess = imageDataAccess;
	}

	public IEnumerable<PackableItemsAsset<PackablePoser3DItem>> ConvertAssets(IEnumerable<KeyValuePair<Image, Poser3DAsset>> assets)
	{
		return assets.Select(ConvertAsset);
	}

	private readonly ConversionSession _session;
	private readonly FileSystemImageDataAccess _imageDataAccess;

	private PackableItemsAsset<PackablePoser3DItem> ConvertAsset(KeyValuePair<Image, Poser3DAsset> assetPair)
	{
		var (image, asset) = assetPair;
		return new PackableItemsAsset<PackablePoser3DItem>
		{
			Items = ConvertItems(asset.Items).ToImmutableArray(),
			ImageId = _imageDataAccess.GetId(image)
		};
	}

	private IEnumerable<PackablePoser3DItem> ConvertItems(IEnumerable<Poser3DItem> items)
	{
		return items.Select(ConvertItem);
	}

	private PackablePoser3DItem ConvertItem(Poser3DItem item) => new()
	{
		TagIndex = _session.TagsIndexes[item.Tag],
		Bounding = item.Bounding,
		KeyPoints = ConvertKeyPoints(item.KeyPoints).ToImmutableArray()
	};

	private IEnumerable<PackableKeyPoint3D> ConvertKeyPoints(IEnumerable<KeyPoint3D> keyPoints)
	{
		return keyPoints.Select(ConvertKeyPoint);
	}

	private PackableKeyPoint3D ConvertKeyPoint(KeyPoint3D keyPoint) => new()
	{
		TagIndex = _session.TagsIndexes[keyPoint.Tag],
		Position = keyPoint.Position,
		IsVisible = keyPoint.IsVisible
	};
}