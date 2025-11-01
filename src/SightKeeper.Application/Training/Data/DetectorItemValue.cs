using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training.Data;

internal sealed class DetectorItemValue : ReadOnlyDetectorItem
{
	public required ReadOnlyTag Tag { get; init; }
	public required Bounding Bounding { get; init; }
}