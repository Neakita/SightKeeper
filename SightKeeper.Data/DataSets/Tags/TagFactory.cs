namespace SightKeeper.Data.Model.DataSets.Tags;

public interface TagFactory<out TTag>
{
	TTag CreateTag(string name);
}