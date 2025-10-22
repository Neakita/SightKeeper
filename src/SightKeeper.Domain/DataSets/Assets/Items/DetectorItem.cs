using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface DetectorItem : TagUser, ReadOnlyDetectorItem
{
	new Bounding Bounding { get; set; }
	new Tag Tag { get; set; }
	ReadOnlyTag ReadOnlyDetectorItem.Tag => Tag;
	Bounding ReadOnlyDetectorItem.Bounding => Bounding;
}