namespace SightKeeper.Data.DataSets.Tags;

internal sealed class TrackableTagsLibrary<TTag>(StorableTagsOwner<TTag> inner, ChangeListener listener) : StorableTagsOwner<TTag>
{
	public IReadOnlyList<TTag> Tags => inner.Tags;

	public TTag CreateTag(string name)
	{
		var tag = inner.CreateTag(name);
		listener.SetDataChanged();
		return tag;
	}

	public void DeleteTagAt(int index)
	{
		inner.DeleteTagAt(index);
		listener.SetDataChanged();
	}

	public void EnsureCapacity(int capacity)
	{
		inner.EnsureCapacity(capacity);
	}
}