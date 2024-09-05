using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Editing;

public interface ExistingTagData : TagData
{
	Tag Tag { get; }
}