using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Detector;

public sealed class DetectorAsset : ItemsAsset<DetectorItem>, ItemsOwner
{
	public DetectorItem CreateItem(Tag tag, Bounding bounding)
	{
		if (tag.Owner != _tagsOwner)
		{
			const string unexpectedTagsOwnerExceptionMessage =
				"When creating a new asset item, a tag from the associated dataset must be used";
			throw new UnexpectedTagsOwnerException(unexpectedTagsOwnerExceptionMessage, _tagsOwner, tag);
		}
		DetectorItem item = new(bounding, tag);
		AddItem(item);
		return item;
	}

	BoundedItem ItemsCreator.CreateItem(Tag tag, Bounding bounding) => CreateItem(tag, bounding);

	internal DetectorAsset(TagsContainer<Tag> tagsOwner)
	{
		_tagsOwner = tagsOwner;
	}

	private readonly TagsContainer<Tag> _tagsOwner;
}