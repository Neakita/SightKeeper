using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Tags;

internal sealed class StorableTagFactory : TagFactory<Tag>
{
	public TagsContainer<Tag>? TagsOwner { get; set; }

	public Tag CreateTag(string name)
	{
		Guard.IsNotNull(TagsOwner);
		return new InMemoryTag
		{
			Name = name,
			Owner = TagsOwner
		};
	}
}