namespace SightKeeper.Application.DataSets.Tags;

public interface PoserTagData : TagData
{
	IReadOnlyCollection<TagData> KeyPointTags { get; }
}