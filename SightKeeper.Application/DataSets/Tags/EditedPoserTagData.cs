using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Tags;

public interface EditedPoserTagData : EditedTagData
{
	new DomainPoserTag Tag { get; }
	DomainTag EditedTagData.Tag => Tag;
	TagsChanges KeyPointTagsChanges { get; }
}