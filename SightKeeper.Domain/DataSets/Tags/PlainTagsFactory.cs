namespace SightKeeper.Domain.DataSets.Tags;

internal sealed class PlainTagsFactory : TagsFactory<Tag>
{
	public static PlainTagsFactory Instance { get; } = new();
	
	public override Tag CreateTag(TagsContainer<Tag> owner, string name)
	{
		return new Tag(owner, name);
	}
}