using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class Poser3DAsset : AbstractItemsAsset<Poser3DItem>, PoserAsset
{
	protected override Poser3DItem CreateItem(DomainTag tag)
	{
		return new Poser3DItem((PoserTag)tag);
	}

	internal Poser3DAsset(TagsContainer<DomainTag> tagsOwner) : base(tagsOwner)
	{
	}

	PoserItem ItemsMaker<PoserItem>.MakeItem(DomainTag tag)
	{
		return MakeItem(tag);
	}

	IReadOnlyList<PoserItem> ItemsContainer<PoserItem>.Items => Items;
}