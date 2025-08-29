using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Data;

public sealed class ItemsAssetDataValue<TItem> : ItemsAssetData<TItem>
{
	public ImageData Image { get; }
	public AssetUsage Usage { get; }
	public IEnumerable<TItem> Items { get; }

	public ItemsAssetDataValue(ImageData image, AssetUsage usage, IEnumerable<TItem> items)
	{
		Image = image;
		Usage = usage;
		Items = items;
	}
}