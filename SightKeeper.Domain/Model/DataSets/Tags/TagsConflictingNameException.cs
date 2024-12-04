namespace SightKeeper.Domain.Model.DataSets.Tags;

public sealed class TagsConflictingNameException : Exception
{
	public Tag EditingTag { get; }
	public string NewName { get; }
	public Tag ConflictingTag { get; }

	public TagsConflictingNameException(Tag editingTag, string newName, Tag conflictingTag)
	{
		EditingTag = editingTag;
		NewName = newName;
		ConflictingTag = conflictingTag;
	}

	public TagsConflictingNameException(string? message, Tag editingTag, string newName, Tag conflictingTag) : base(message)
	{
		EditingTag = editingTag;
		NewName = newName;
		ConflictingTag = conflictingTag;
	}

	public TagsConflictingNameException(string? message, Exception? innerException, Tag editingTag, string newName, Tag conflictingTag) : base(message, innerException)
	{
		EditingTag = editingTag;
		NewName = newName;
		ConflictingTag = conflictingTag;
	}
}