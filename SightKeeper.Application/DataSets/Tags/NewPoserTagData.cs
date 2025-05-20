namespace SightKeeper.Application.DataSets.Tags;

public interface NewPoserTagData : NewTagData
{
	IEnumerable<NewTagData> KeyPointTags { get; }
}