using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Poser;

public sealed class DomainPoserAsset(PoserAsset inner, TagsContainer<DomainPoserTag> tagsOwner) : PoserAsset
{
	public ManagedImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set => inner.Usage = value;
	}

	public IReadOnlyList<PoserItem> Items => inner.Items;

	public PoserItem MakeItem(PoserTag tag)
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