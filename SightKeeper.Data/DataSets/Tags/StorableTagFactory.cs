using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class StorableTagFactory : TagFactory<StorableTag>
{
	public TagsContainer<Tag>? TagsOwner { get; set; }

	public StorableTag CreateTag(string name)
	{
		Guard.IsNotNull(TagsOwner);
		return new InMemoryTag
		{
			Name = name,
			Owner = TagsOwner
		};
	}
}