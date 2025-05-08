namespace SightKeeper.Domain.DataSets.Tags;

internal abstract class TagsFactory<TTag> where TTag : Tag
{
	public abstract TTag CreateTag(TagsContainer<Tag> owner, string name);
}