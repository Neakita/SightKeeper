namespace SightKeeper.Application.DataSets.Tags;

public interface NewPoserTagData : NewTagData
{
	IReadOnlyCollection<NewTagData> KeyPointTags { get; }
}