using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface ReadOnlyDetectorItem
{
	ReadOnlyTag Tag { get; }
	Bounding Bounding { get; }
}