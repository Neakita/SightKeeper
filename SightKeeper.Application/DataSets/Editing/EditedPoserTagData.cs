using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Editing;

public interface EditedPoserTagData : EditedTagData
{
	IReadOnlyCollection<TagData> KeyPointTags { get; }
}