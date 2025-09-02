using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class StorableTagsOwnerExtension<TTag, TExtendedTag>(TagsOwner<TTag> tagsOwner, StorableTagsOwner<TExtendedTag> extendedTagsOwner) : StorableTagsOwner<TExtendedTag>
	where TTag : notnull
	where TExtendedTag : TTag
{
	public IReadOnlyList<TExtendedTag> Tags => (IReadOnlyList<TExtendedTag>)tagsOwner.Tags;

	public TExtendedTag CreateTag(string name)
	{
		return (TExtendedTag)tagsOwner.CreateTag(name);
	}

	public void DeleteTagAt(int index)
	{
		tagsOwner.DeleteTagAt(index);
	}

	public void EnsureCapacity(int capacity)
	{
		extendedTagsOwner.EnsureCapacity(capacity);
	}
}