namespace SightKeeper.Data.DataSets.Tags;

internal interface TagFactory<out TTag>
{
	TTag CreateTag(string name);
}