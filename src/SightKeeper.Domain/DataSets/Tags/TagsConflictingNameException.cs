using System.Diagnostics.CodeAnalysis;

namespace SightKeeper.Domain.DataSets.Tags;

public sealed class TagsConflictingNameException : Exception
{
	public static void ThrowIfNameConflicts(IEnumerable<Tag> siblings, Tag editingTag, string newName)
	{
		foreach (var sibling in siblings)
			ThrowIfNameConflicts(sibling, editingTag, newName);
	}

	private static void ThrowIfNameConflicts(Tag sibling, Tag editingTag, string newName)
	{
		if (sibling.Name == newName && sibling != editingTag)
			Throw(sibling, editingTag, newName);
	}

	[DoesNotReturn]
	private static void Throw(Tag sibling, Tag editingTag, string newName)
	{
		const string tagsConflictingNameExceptionMessage =
			"An attempt has been made to assign a name already occupied by another tag. " +
			"Before setting a new name, " +
			"make sure that this name is not occupied by another tag in the appropriate context (dataset or key point tag)";
		throw new TagsConflictingNameException(tagsConflictingNameExceptionMessage, editingTag, newName, sibling);
	}

	public Tag EditingTag { get; }
	public string Name { get; }
	public Tag ConflictingTag { get; }

	public TagsConflictingNameException(string? message, Tag editingTag, string name, Tag conflictingTag) :
		base(message)
	{
		EditingTag = editingTag;
		Name = name;
		ConflictingTag = conflictingTag;
	}
}