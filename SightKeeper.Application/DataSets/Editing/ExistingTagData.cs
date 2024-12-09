using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Editing;

public interface ExistingTagData : TagData
{
	Tag Tag { get; }
}