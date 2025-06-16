using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Domain.Images;

public interface Image
{
	DateTimeOffset CreationTimestamp { get; }
	Vector2<ushort> Size { get; }
	IReadOnlyCollection<Asset> Assets { get; }
	bool IsInUse => Assets.Count > 0;

	Stream? OpenReadDataStream();
	Stream? OpenWriteDataStream();
}