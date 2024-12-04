namespace SightKeeper.Domain.Model.DataSets.Tags;

public sealed class InappropriateTagsOwnerChangeException : Exception
{
	public static void ThrowIfOwnerChanged(Tag currentTag, Tag newTag)
	{
		if (currentTag.Owner != newTag.Owner)
			Throw(currentTag, newTag);
	}

	private static void Throw(Tag currentTag, Tag causer)
	{
		throw new InappropriateTagsOwnerChangeException(InappropriateTagsOwnerChangeExceptionMessage, currentTag, causer);
	}

	private const string InappropriateTagsOwnerChangeExceptionMessage =
		"The classifier asset cannot change the tags owner " +
		"because this will mean that another dataset has become the owner of this asset. " +
		"Use tags from the current dataset only.";

	public Tag CurrentTag { get; }
	public Tag Causer { get; }

	public InappropriateTagsOwnerChangeException(string? message, Tag currentTag, Tag causer) : base(message)
	{
		CurrentTag = currentTag;
		Causer = causer;
	}
}