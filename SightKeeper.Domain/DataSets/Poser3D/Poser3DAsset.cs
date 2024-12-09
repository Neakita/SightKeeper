using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class Poser3DAsset : ItemsAsset<Poser3DItem>
{
	public Poser3DItem CreateItem(PoserTag tag, Bounding bounding)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(_tagsOwner, tag);
		Poser3DItem item = new(bounding, tag);
		AddItem(item);
		return item;
	}

	internal Poser3DAsset(TagsOwner tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	private readonly TagsOwner _tagsOwner;
}