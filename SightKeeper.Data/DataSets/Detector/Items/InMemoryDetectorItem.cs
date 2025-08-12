using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class InMemoryDetectorItem(Bounding bounding, StorableTag tag) : StorableDetectorItem
{
	public Bounding Bounding { get; set; } = bounding;
	public StorableTag Tag { get; set; } = tag;
	public StorableDetectorItem Innermost => this;
}