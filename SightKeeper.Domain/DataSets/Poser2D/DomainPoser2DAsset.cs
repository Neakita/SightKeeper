using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class DomainPoser2DAsset(ItemsAsset<Poser2DItem> inner, TagsContainer<DomainPoserTag> tagsOwner) : PoserAsset<Poser2DItem>
{
	public ManagedImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set => inner.Usage = value;
	}

	public IReadOnlyList<Poser2DItem> Items => inner.Items;

	public Poser2DItem MakeItem(PoserTag tag)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(tagsOwner, tag);
		return inner.MakeItem(tag);
	}

	public void DeleteItemAt(int index)
	{
		inner.DeleteItemAt(index);
	}

	public void ClearItems()
	{
		inner.ClearItems();
	}
}