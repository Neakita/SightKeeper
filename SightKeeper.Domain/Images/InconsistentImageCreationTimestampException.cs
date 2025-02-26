namespace SightKeeper.Domain.Images;

public sealed class InconsistentImageCreationTimestampException : Exception
{
	public DateTimeOffset NewImageCreationTimestamp { get; }
	public ImageSet AffectedSet { get; }

	public InconsistentImageCreationTimestampException(string? message, DateTimeOffset newImageCreationTimestamp, ImageSet affectedSet) : base(message)
	{
		NewImageCreationTimestamp = newImageCreationTimestamp;
		AffectedSet = affectedSet;
	}
}