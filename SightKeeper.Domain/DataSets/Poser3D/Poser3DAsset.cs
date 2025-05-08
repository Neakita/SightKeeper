using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class Poser3DAsset : ItemsAsset<Poser3DItem>, ItemsOwner
{
	public Poser3DItem CreateItem(PoserTag tag, Bounding bounding)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(_tagsOwner, tag);
		Poser3DItem item = new(bounding, tag);
		AddItem(item);
		return item;
	}

	BoundedItem ItemsCreator.CreateItem(Tag tag, Bounding bounding)
	{
		return CreateItem((PoserTag)tag, bounding);
	}

	internal Poser3DAsset(TagsContainer<Tag> tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	private readonly TagsContainer<Tag> _tagsOwner;
}