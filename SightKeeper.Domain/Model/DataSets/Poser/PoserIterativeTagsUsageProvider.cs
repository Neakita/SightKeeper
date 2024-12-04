using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

internal class PoserIterativeTagsUsageProvider<TItem> : IterativeTagsUsageProvider<ItemsAsset<TItem>> where TItem : PoserItem
{
	protected override bool IsInUse(ItemsAsset<TItem> asset, Tag tag)
	{
		if (tag is PoserTag poserTag)
			return asset.Items.Any(item => item.Tag == poserTag);
		return asset.Items
			.Where(item => item.Tag == tag.Owner)
			.SelectMany(item => item.KeyPoints)
			.Any(keyPoint => keyPoint.Tag == tag);
	}
}