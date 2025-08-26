using SightKeeper.Domain;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training.Data;

public interface ImageData
{
	Vector2<ushort> Size { get; }
	DateTimeOffset CreationTimestamp { get; }
	Image Image { get; }
}