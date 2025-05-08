namespace SightKeeper.Domain.DataSets.Tags;

public sealed class TagsConflictingNameException : Exception
{
	public Tag EditingTag { get; }
	public string Name { get; }
	public Tag ConflictingTag { get; }

	public TagsConflictingNameException(string? message, Tag editingTag, string name, Tag conflictingTag) : base(message)
	{
		EditingTag = editingTag;
		Name = name;
		ConflictingTag = conflictingTag;
	}
}