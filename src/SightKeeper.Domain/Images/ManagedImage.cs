using FlakeId;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Domain.Images;

public interface ManagedImage : ImageData, IDisposable
{
	Id Id { get; }
	IReadOnlyCollection<Asset> Assets { get; }
	bool IsInUse => Assets.Count > 0;
	string? DataFormat { get; }

	void AddAsset(Asset asset);
	void RemoveAsset(Asset asset);
	Stream? OpenWriteStream();
	Stream? OpenReadStream();
	void DeleteData();
	bool TryCopyTo(string filePath);
}