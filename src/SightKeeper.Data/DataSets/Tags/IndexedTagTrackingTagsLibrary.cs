using SightKeeper.Data.DataSets.Tags.Decorators;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class IndexedTagTrackingTagsLibrary<T>(TagsOwner<T> inner) : TagsOwner<T>, Decorator<TagsOwner<T>> where T : notnull
{
	public IReadOnlyList<T> Tags => inner.Tags;
	public TagsOwner<T> Inner => inner;

	public T CreateTag(string name)
	{
		var newTagIndex = Tags.Count;
		var tag = inner.CreateTag(name);
		SetTagIndex(tag, newTagIndex);
		return tag;
	}

	public void DeleteTagAt(int index)
	{
		inner.DeleteTagAt(index);
		ShiftRemainingTags(index);
	}

	private static void SetTagIndex(T tag, int newTagIndex)
	{
		var indexHolder = tag.GetFirst<TagIndexHolder>();
		indexHolder.Index = (byte)newTagIndex;
	}

	private void ShiftRemainingTags(int index)
	{
		for (int i = index; i < Tags.Count; i++)
		{
			var tag = Tags[i];
			var indexHolder = tag.GetFirst<TagIndexHolder>();
			indexHolder.Index--;
		}
	}
}