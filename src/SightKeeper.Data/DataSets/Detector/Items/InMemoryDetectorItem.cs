using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class InMemoryDetectorItem(Bounding bounding, Tag tag) : DetectorItem
{
	public Bounding Bounding { get; set; } = bounding;
	public Tag Tag { get; set; } = tag;
}