namespace SightKeeper.Domain.Model.DataSets.Tags;

public sealed class UnexpectedTagsOwnerException : Exception
{
	public static void ThrowIfTagsOwnerDoesNotMatch(TagsOwner tagsOwner, Tag tag)
	{
		if (tagsOwner == tag.Owner)
			return;
		const string unexpectedTagsOwnerExceptionMessage =
			"A tag from the associated context (dataset or poser tag) must be used";
		throw new UnexpectedTagsOwnerException(unexpectedTagsOwnerExceptionMessage, tagsOwner, tag);
	}

	public TagsOwner ExpectedOwner { get; }
	public Tag Causer { get; }

	public UnexpectedTagsOwnerException(string? message, TagsOwner expectedOwner, Tag causer) : base(message)
	{
		ExpectedOwner = expectedOwner;
		Causer = causer;
	}
}