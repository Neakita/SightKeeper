using SightKeeper.Data.DataSets.Tags.Decorators;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Tags.Decorators;

internal sealed class IndexedTagTrackingPoserTag(PoserTag inner) : PoserTag, Decorator<PoserTag>
{
	public TagsContainer<PoserTag> Owner => inner.Owner;
	public IReadOnlyCollection<TagUser> Users => inner.Users;
	public IReadOnlyList<Tag> KeyPointTags => inner.KeyPointTags;
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
		var newTagIndex = KeyPointTags.Count;
		var tag = inner.CreateKeyPointTag(name);
		SetTagIndex(tag, newTagIndex);
		return tag;
	}

	public void DeleteKeyPointTagAt(int index)
	{
		inner.DeleteKeyPointTagAt(index);
		ShiftRemainingTags(index);
	}

	public void DeleteKeyPointTag(Tag tag)
	{
		var index = KeyPointTags.Index().First(tuple => tuple.Item == tag).Index;
		DeleteKeyPointTagAt(index);
	}

	private static void SetTagIndex(Tag tag, int newTagIndex)
	{
		var indexHolder = tag.GetFirst<TagIndexHolder>();
		indexHolder.Index = (byte)newTagIndex;
	}

	private void ShiftRemainingTags(int index)
	{
		for (int i = index; i < KeyPointTags.Count; i++)
		{
			var tag = KeyPointTags[i];
			var indexHolder = tag.GetFirst<TagIndexHolder>();
			indexHolder.Index--;
		}
	}
}