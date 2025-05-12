using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class Poser2DAsset : AbstractItemsAsset<Poser2DItem>, PoserAsset
{
	protected override Poser2DItem CreateItem(Tag tag, Bounding bounding)
	{
		return new Poser2DItem(bounding, (PoserTag)tag);
	}

	internal Poser2DAsset(TagsContainer<Tag> tagsOwner) : base(tagsOwner)
	{
	}

	PoserItem ItemsMaker<PoserItem>.MakeItem(Tag tag, Bounding bounding)
	{
		return MakeItem(tag, bounding);
	}

	IReadOnlyList<PoserItem> ItemsContainer<PoserItem>.Items => Items;
}