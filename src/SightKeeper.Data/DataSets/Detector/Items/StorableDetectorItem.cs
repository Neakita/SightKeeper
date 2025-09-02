using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector.Items;

public interface StorableDetectorItem : DetectorItem
{
	new StorableTag Tag { get; set; }
	StorableDetectorItem Innermost { get; }

	Tag DetectorItem.Tag
	{
		get => Tag;
		set => Tag = (StorableTag)value;
	}
}