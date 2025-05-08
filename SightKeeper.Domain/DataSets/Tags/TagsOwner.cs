namespace SightKeeper.Domain.DataSets.Tags;

public interface TagsOwner<out TTag> : TagsContainer<TTag>
{
	TTag CreateTag(string name);
	void DeleteTagAt(int index);
}