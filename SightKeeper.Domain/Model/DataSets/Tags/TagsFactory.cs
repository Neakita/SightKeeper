namespace SightKeeper.Domain.Model.DataSets.Tags;

public abstract class TagsFactory<TTag> where TTag : Tag
{
	public abstract TTag CreateTag(TagsOwner owner, string name);
}