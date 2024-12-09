namespace SightKeeper.Domain.DataSets.Tags;

internal sealed class PlainTagsFactory : TagsFactory<Tag>
{
	public static PlainTagsFactory Instance { get; } = new();
	
	public override Tag CreateTag(TagsOwner owner, string name)
	{
		return new Tag(owner, name);
	}
}