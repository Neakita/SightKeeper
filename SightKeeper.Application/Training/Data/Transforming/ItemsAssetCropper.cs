using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data.Transforming;

public sealed class ItemsAssetCropper<TItem>(ItemCropper<TItem> itemCropper) : AssetCropper<ItemsAssetData<TItem>>
	where TItem : AssetItemData
{
	public ItemsAssetData<TItem> CropAsset(ItemsAssetData<TItem> asset, Rectangle cropRectangle)
	{
		var image = new CroppedImageData(asset.Image, cropRectangle);
		var items = itemCropper.CropItems(asset.Items, cropRectangle, asset.Image.Size).ToList();
		return new ItemsAssetDataValue<TItem>(image, asset.Usage, items);
	}
}