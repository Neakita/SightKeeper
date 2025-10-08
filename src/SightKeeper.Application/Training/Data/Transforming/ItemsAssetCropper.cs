using SightKeeper.Domain.DataSets.Assets.Items;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data.Transforming;

internal sealed class ItemsAssetCropper<TItem>(ItemCropper<TItem> itemCropper) : AssetCropper<ReadOnlyItemsAsset<TItem>>
	where TItem : ReadOnlyAssetItem
{
	public ReadOnlyItemsAsset<TItem> CropAsset(ReadOnlyItemsAsset<TItem> asset, Rectangle cropRectangle)
	{
		var image = new CroppedImageData(asset.Image, cropRectangle);
		var items = itemCropper.CropItems(asset.Items, cropRectangle, asset.Image.Size).ToList();
		return new ReadOnlyItemsAssetValue<TItem>(image, asset.Usage, items);
	}
}