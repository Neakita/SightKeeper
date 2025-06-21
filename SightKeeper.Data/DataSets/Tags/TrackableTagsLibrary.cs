using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class TrackableTagsLibrary<TTag>(TagsOwner<TTag> inner, ChangeListener listener) : TagsOwner<TTag>
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
}