using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Training.Data;

internal sealed class ReadOnlyItemsAssetValue<TItem> : ReadOnlyItemsAsset<TItem>
{
	public ImageData Image { get; }
	public AssetUsage Usage { get; }
	public IEnumerable<TItem> Items { get; }

	public ReadOnlyItemsAssetValue(ImageData image, AssetUsage usage, IEnumerable<TItem> items)
	{
		Image = image;
		Usage = usage;
		Items = items;
	}
}