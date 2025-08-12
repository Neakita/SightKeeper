namespace SightKeeper.Data.DataSets.Tags;

internal sealed class LockingTagsLibrary<TTag>(StorableTagsOwner<TTag> inner, Lock editingLock) : StorableTagsOwner<TTag>
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

	public void EnsureCapacity(int capacity)
	{
		inner.EnsureCapacity(capacity);
	}
}