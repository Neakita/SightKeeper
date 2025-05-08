namespace SightKeeper.Domain.DataSets.Tags;

public class Tag
{
	public string Name
	{
		get;
		set
		{
			const string tagsConflictingNameExceptionMessage =
				"An attempt has been made to assign a name already occupied by another tag. " +
				"Before setting a new name, " +
				"make sure that this name is not occupied by another tag in the appropriate context (dataset or key point tag)";
			foreach (var sibling in Owner.Tags)
				if (sibling.Name == value && sibling != this)
					throw new TagsConflictingNameException(tagsConflictingNameExceptionMessage, this, value, sibling);
			field = value;
		}
	}
	public uint Color { get; set; }

	public TagsContainer<Tag> Owner { get; }

	internal Tag(TagsContainer<Tag> owner, string name)
	{
		Owner = owner;
		Name = name;
	}
}