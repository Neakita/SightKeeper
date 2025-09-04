using SightKeeper.Domain.DataSets.Tags;
using Vibrance.Changes;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class ObservableTagsLibrary<TTag>(TagsOwner<TTag> inner) : TagsOwner<TTag>
{
	public IReadOnlyList<TTag> Tags => _tags;

	public TTag CreateTag(string name)
	{
		var index = _tags.Count;
		var tag = inner.CreateTag(name);
		Insertion<TTag> change = new()
		{
			Index = index,
			Items = [tag]
		};
		_tags.Notify(change);
		return tag;
	}

	public void DeleteTagAt(int index)
	{
		var tag = _tags[index];
		inner.DeleteTagAt(index);
		IndexedRemoval<TTag> change = new()
		{
			Index = index,
			Items = [tag]
		};
		_tags.Notify(change);
	}

	private readonly ExternalObservableList<TTag> _tags = new(inner.Tags);
}