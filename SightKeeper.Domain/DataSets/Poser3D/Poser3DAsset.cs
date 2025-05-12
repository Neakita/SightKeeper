using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class Poser3DAsset : AbstractItemsAsset<Poser3DItem>, PoserAsset
{
	protected override Poser3DItem CreateItem(Tag tag, Bounding bounding)
	{
		return new Poser3DItem(bounding, (PoserTag)tag);
	}

	internal Poser3DAsset(TagsContainer<Tag> tagsOwner) : base(tagsOwner)
	{
	}

	PoserItem ItemsMaker<PoserItem>.MakeItem(Tag tag, Bounding bounding)
	{
		return MakeItem(tag, bounding);
	}

	IReadOnlyList<PoserItem> ItemsContainer<PoserItem>.Items => Items;
}