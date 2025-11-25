using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class InMemoryTagsLibrary<TTag>(TagFactory<TTag> tagFactory) : TagsOwner<TTag>, PostWrappingInitializable<DataSet<TTag, ReadOnlyAsset>>
{
	public IReadOnlyList<TTag> Tags => _tags;

	public void Initialize(DataSet<TTag, ReadOnlyAsset> wrapped)
	{
		if (tagFactory is PostWrappingInitializable<DataSet<TTag, ReadOnlyAsset>> initializable)
			initializable.Initialize(wrapped);
	}

	public TTag CreateTag(string name)
	{
		var tag = tagFactory.CreateTag(name);
		_tags.Add(tag);
		return tag;
	}

	public void DeleteTagAt(int index)
	{
		_tags.RemoveAt(index);
	}

	public void EnsureCapacity(int capacity)
	{
		_tags.EnsureCapacity(capacity);
	}

	private readonly List<TTag> _tags = new();
}