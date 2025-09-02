using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

public interface StorableTagsOwner<out TTag> : TagsOwner<TTag>
{
	void EnsureCapacity(int capacity);
}