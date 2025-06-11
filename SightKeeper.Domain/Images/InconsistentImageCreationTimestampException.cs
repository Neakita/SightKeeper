namespace SightKeeper.Domain.Images;

public sealed class InconsistentImageCreationTimestampException : Exception
{
	public DateTimeOffset NewImageCreationTimestamp { get; }
	public DomainImageSet AffectedSet { get; }

	public InconsistentImageCreationTimestampException(string? message, DateTimeOffset newImageCreationTimestamp, DomainImageSet affectedSet) : base(message)
	{
		NewImageCreationTimestamp = newImageCreationTimestamp;
		AffectedSet = affectedSet;
	}
}