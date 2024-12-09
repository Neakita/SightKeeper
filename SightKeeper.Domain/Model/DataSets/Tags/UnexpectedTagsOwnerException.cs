namespace SightKeeper.Domain.Model.DataSets.Tags;

public sealed class UnexpectedTagsOwnerException : Exception
{
	internal static void ThrowIfTagsOwnerDoesNotMatch(TagsOwner tagsOwner, Tag tag)
	{
		if (tagsOwner == tag.Owner)
			return;
		const string unexpectedTagsOwnerExceptionMessage =
			"A tag from the associated context (dataset or poser tag) must be used";
		throw new UnexpectedTagsOwnerException(unexpectedTagsOwnerExceptionMessage, tagsOwner, tag);
	}

	public Tag Causer { get; }

	internal TagsOwner ExpectedOwner { get; }

	internal UnexpectedTagsOwnerException(string? message, TagsOwner expectedOwner, Tag causer) : base(message)
	{
		ExpectedOwner = expectedOwner;
		Causer = causer;
	}
}