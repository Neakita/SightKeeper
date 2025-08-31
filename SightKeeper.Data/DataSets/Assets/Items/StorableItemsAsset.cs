using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets.Items;

public interface StorableItemsAsset<out TItem> : ItemsAsset<TItem>
{
	new StorableImage Image { get; }
	StorableItemsAsset<TItem> Innermost { get; }

	TItem MakeItem(StorableTag tag);

	ManagedImage Asset.Image => Image;

	TItem ItemsMaker<TItem>.MakeItem(Tag tag)
	{
		return MakeItem((StorableTag)tag);
	}
}