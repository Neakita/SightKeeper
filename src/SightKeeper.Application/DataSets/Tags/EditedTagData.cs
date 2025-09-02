using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Tags;

public interface EditedTagData
{
	Tag Tag { get; }
	string Name { get; }
	uint Color { get; }
}