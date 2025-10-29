using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data.Transforming;

internal sealed class ItemsAssetCropper<TItem>(ItemCropper<TItem> itemCropper) : AssetCropper<ReadOnlyItemsAsset<TItem>>
	where TItem : ReadOnlyDetectorItem
{
	public ReadOnlyItemsAsset<TItem> CropAsset(ReadOnlyItemsAsset<TItem> asset, Rectangle cropRectangle)
	{
		var loadableImage = asset.Image.GetFirst<LoadableImage>();
		var image = new CroppedImageData(asset.Image, loadableImage, cropRectangle);
		var items = itemCropper.CropItems(asset.Items, cropRectangle, asset.Image.Size).ToList();
		return new ReadOnlyItemsAssetValue<TItem>(image, asset.Usage, items);
	}
}