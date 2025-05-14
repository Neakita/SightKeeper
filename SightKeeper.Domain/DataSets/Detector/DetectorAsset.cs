using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Detector;

public sealed class DetectorAsset : AbstractItemsAsset<DetectorItem>
{
	protected override DetectorItem CreateItem(Tag tag)
	{
		return new DetectorItem(tag);
	}

	internal DetectorAsset(TagsContainer<Tag> tagsOwner) : base(tagsOwner)
	{
	}
}