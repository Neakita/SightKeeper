using MemoryPack;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

[MemoryPackable]
internal sealed partial class PackableTagsLibrary<TTag> : TagsOwner<TTag> where TTag : new()
{
	[MemoryPackIgnore] public IReadOnlyList<TTag> Tags => _tags;

	public TTag CreateTag(string name)
	{
		TTag tag = new();
		_tags.Add(tag);
		return tag;
	}

	public void DeleteTagAt(int index)
	{
		_tags.RemoveAt(index);
	}

	public PackableTagsLibrary()
	{
		_tags = new List<TTag>();
	}

	[MemoryPackConstructor]
	public PackableTagsLibrary(List<TTag> tags)
	{
		_tags = tags;
	}

	[MemoryPackInclude]
	private readonly List<TTag> _tags;
}