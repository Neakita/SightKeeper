using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Tags;

public interface EditedPoserTagData : EditedTagData
{
	new PoserTag Tag { get; }
	Tag EditedTagData.Tag => Tag;
	TagsChanges KeyPointTagsChanges { get; }
}