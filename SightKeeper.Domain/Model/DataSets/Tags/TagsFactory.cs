namespace SightKeeper.Domain.Model.DataSets.Tags;

public interface TagsFactory<TTag> where TTag : Tag, TagsFactory<TTag>
{
	static abstract TTag Create(string name, TagsLibrary<TTag> library);
}