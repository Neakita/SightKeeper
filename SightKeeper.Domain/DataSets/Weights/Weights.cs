using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public interface Weights : TagUser
{
	WeightsMetadata Metadata { get; }
	IReadOnlyList<Tag> Tags { get; }

	Stream? OpenWriteSteam();
	Stream? OpenReadStream();
}