namespace SightKeeper.Data.DataSets.Tags;

public interface TagFactory<out TTag>
{
	TTag CreateTag(string name);
}