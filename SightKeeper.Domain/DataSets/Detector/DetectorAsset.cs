using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Detector;

public sealed class DetectorAsset : AbstractItemsAsset<DetectorItem>
{
	protected override DetectorItem CreateItem(Tag tag, Bounding bounding)
	{
		return new DetectorItem(bounding, tag);
	}

	internal DetectorAsset(TagsContainer<Tag> tagsOwner) : base(tagsOwner)
	{
	}
}