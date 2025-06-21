using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class LockingTagsLibrary<TTag>(TagsOwner<TTag> inner, Lock editingLock) : TagsOwner<TTag>
{
	public IReadOnlyList<TTag> Tags => inner.Tags;

	public TTag CreateTag(string name)
	{
		lock (editingLock)
			return inner.CreateTag(name);
	}

	public void DeleteTagAt(int index)
	{
		lock (editingLock)
			inner.DeleteTagAt(index);
	}
}