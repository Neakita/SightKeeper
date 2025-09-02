using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Data.DataSets.Detector.Items;

internal sealed class StorableDetectorItemExtension(DetectorItem inner, StorableDetectorItem innerExtended) : StorableDetectorItem
{
	public Bounding Bounding
	{
		get => inner.Bounding;
		set => inner.Bounding = value;
	}

	public StorableDetectorItem Innermost => innerExtended.Innermost;

	public StorableTag Tag
	{
		get => (StorableTag)inner.Tag;
		set => inner.Tag = value;
	}
}