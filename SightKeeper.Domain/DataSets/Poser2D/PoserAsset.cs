using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser2D;

public interface PoserAsset<out TItem> : ItemsAsset<TItem> where TItem : PoserItem
{
	TItem MakeItem(DomainPoserTag tag);
	TItem ItemsMaker<TItem>.MakeItem(Tag tag)
	{
		return MakeItem((DomainPoserTag)tag);
	}
}