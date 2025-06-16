using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Tags;

public interface EditedTagData
{
	DomainTag Tag { get; }
	string Name { get; }
	uint Color { get; }
}