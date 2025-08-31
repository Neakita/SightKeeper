using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Domain.Images;

public interface ManagedImage : ImageData
{
	IReadOnlyCollection<Asset> Assets { get; }
	bool IsInUse => Assets.Count > 0;
	string? DataFormat { get; }

	Stream? OpenWriteStream();
	Stream? OpenReadStream();
	bool TryCopyTo(string filePath);
}