namespace SightKeeper.Domain.Images;

public interface ImageData
{
	Vector2<ushort> Size { get; }
	DateTimeOffset CreationTimestamp { get; }
}