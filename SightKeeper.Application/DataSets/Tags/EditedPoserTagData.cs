namespace SightKeeper.Application.DataSets.Tags;

public interface EditedPoserTagData : EditedTagData
{
	TagsChanges KeyPointTagsChanges { get; }
}