using FlakeId;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public interface WeightsData : TagUser
{
	Id Id { get; }
	WeightsMetadata Metadata { get; }
	IReadOnlyList<Tag> Tags { get; }

	Stream? OpenWriteSteam();
	Stream? OpenReadStream();
	void DeleteData();
}