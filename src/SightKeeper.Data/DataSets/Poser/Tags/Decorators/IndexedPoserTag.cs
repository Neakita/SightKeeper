using SightKeeper.Data.DataSets.Tags.Decorators;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Tags.Decorators;

internal sealed class IndexedPoserTag(PoserTag inner) : PoserTag, TagIndexHolder, Decorator<PoserTag>
{
	public TagsContainer<PoserTag> Owner => inner.Owner;
	public IReadOnlyCollection<TagUser> Users => inner.Users;
	public IReadOnlyList<Tag> KeyPointTags => inner.KeyPointTags;
	public byte Index { get; set; }
	public PoserTag Inner => inner;

	public string Name
	{
		get => inner.Name;
		set => inner.Name = value;
	}

	public uint Color
	{
		get => inner.Color;
		set => inner.Color = value;
	}

	public Tag CreateKeyPointTag(string name)
	{
		return inner.CreateKeyPointTag(name);
	}

	public void DeleteKeyPointTagAt(int index)
	{
		inner.DeleteKeyPointTagAt(index);
	}

	public void DeleteKeyPointTag(Tag tag)
	{
		inner.DeleteKeyPointTag(tag);
	}
}