using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public interface PoserTag : Tag, TagsOwner<Tag>
{
	IReadOnlyList<Tag> KeyPointTags { get; }
	new TagsContainer<PoserTag> Owner { get; }
	TagsContainer<Tag> Tag.Owner => Owner;
	Tag CreateKeyPointTag(string name);
	void DeleteKeyPointTagAt(int index);
	void DeleteKeyPointTag(Tag tag);

	IReadOnlyList<Tag> TagsContainer<Tag>.Tags => KeyPointTags;

	Tag TagsOwner<Tag>.CreateTag(string name)
	{
		return CreateKeyPointTag(name);
	}

	void TagsOwner<Tag>.DeleteTagAt(int index)
	{
		DeleteKeyPointTagAt(index);
	}
}