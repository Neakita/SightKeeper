using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class Poser2DAsset : ItemsAsset<Poser2DItem>, ItemsOwner
{
	public Poser2DItem CreateItem(PoserTag tag, Bounding bounding)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(_tagsOwner, tag);
		Poser2DItem item = new(bounding, tag);
		AddItem(item);
		return item;
	}

	BoundedItem ItemsCreator.CreateItem(Tag tag, Bounding bounding)
	{
		return CreateItem((PoserTag)tag, bounding);
	}

	internal Poser2DAsset(TagsOwner tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	private readonly TagsOwner _tagsOwner;
}