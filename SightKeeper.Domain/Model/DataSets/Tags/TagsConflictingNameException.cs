namespace SightKeeper.Domain.Model.DataSets.Tags;

public sealed class TagsConflictingNameException : Exception
{
	public Tag EditingTag { get; }
	public string NewName { get; }
	public Tag ConflictingTag { get; }

	public TagsConflictingNameException(string? message, Tag editingTag, string newName, Tag conflictingTag) : base(message)
	{
		EditingTag = editingTag;
		NewName = newName;
		ConflictingTag = conflictingTag;
	}
}