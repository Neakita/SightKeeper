using System.Diagnostics.CodeAnalysis;

namespace SightKeeper.Domain.DataSets.Tags;

public sealed class TagIsInUseException : Exception
{
	public static void ThrowForDeletionIfInUse(Tag tag)
	{
		if (tag.Users.Count > 0)
			ThrowForDeletion(tag);
	}

	[DoesNotReturn]
	public static void ThrowForDeletion(Tag tag)
	{
		const string message =
			"The tag is used by some asset, so it cannot be deleted. " +
			"Delete the affected assets or their affected items before attempting to delete the tag";
		throw new TagIsInUseException(message, tag);
	}

	public Tag Tag { get; }

	public TagIsInUseException(string? message, Tag tag) : base(message)
	{
		Tag = tag;
	}
}