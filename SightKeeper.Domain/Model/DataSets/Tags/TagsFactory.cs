namespace SightKeeper.Domain.Model.DataSets.Tags;

internal abstract class TagsFactory<TTag> where TTag : Tag
{
	public abstract TTag CreateTag(TagsOwner owner, string name);
}