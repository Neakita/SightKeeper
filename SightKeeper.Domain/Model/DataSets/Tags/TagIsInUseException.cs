using System.Diagnostics.CodeAnalysis;

namespace SightKeeper.Domain.Model.DataSets.Tags;

public sealed class TagIsInUseException : Exception
{
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