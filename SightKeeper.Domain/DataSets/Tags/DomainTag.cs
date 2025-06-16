namespace SightKeeper.Domain.DataSets.Tags;

public sealed class DomainTag(Tag inner) : Tag
{
	public string Name
	{
		get => inner.Name;
		set
		{
			const string tagsConflictingNameExceptionMessage =
				"An attempt has been made to assign a name already occupied by another tag. " +
				"Before setting a new name, " +
				"make sure that this name is not occupied by another tag in the appropriate context (dataset or key point tag)";
			foreach (var sibling in Owner.Tags)
				if (sibling.Name == value && sibling != this)
					throw new TagsConflictingNameException(tagsConflictingNameExceptionMessage, this, value, sibling);
			inner.Name = value;
		}
	}

	public uint Color
	{
		get => inner.Color;
		set => inner.Color = value;
	}

	public TagsContainer<Tag> Owner => inner.Owner;
	public IReadOnlyCollection<TagUser> Users => inner.Users;
}