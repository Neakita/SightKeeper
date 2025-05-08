namespace SightKeeper.Domain.DataSets.Tags;

public sealed class UnexpectedTagsOwnerException : Exception
{
	internal static void ThrowIfTagsOwnerDoesNotMatch(TagsContainer<Tag> tagsOwner, Tag tag)
	{
		if (tagsOwner == tag.Owner)
			return;
		const string unexpectedTagsOwnerExceptionMessage =
			"A tag from the associated context (dataset or poser tag) must be used";
		throw new UnexpectedTagsOwnerException(unexpectedTagsOwnerExceptionMessage, tagsOwner, tag);
	}

	public Tag Causer { get; }

	internal TagsContainer<Tag> ExpectedOwner { get; }

	internal UnexpectedTagsOwnerException(string? message, TagsContainer<Tag> expectedOwner, Tag causer) : base(message)
	{
		ExpectedOwner = expectedOwner;
		Causer = causer;
	}
}