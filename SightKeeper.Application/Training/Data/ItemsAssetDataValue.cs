namespace SightKeeper.Application.Training.Data;

public sealed class ItemsAssetDataValue<TItem> : ItemsAssetData<TItem>
{
	public ImageData Image { get; }
	public IEnumerable<TItem> Items { get; }

	public ItemsAssetDataValue(ImageData image, IEnumerable<TItem> items)
	{
		Image = image;
		Items = items;
	}
}