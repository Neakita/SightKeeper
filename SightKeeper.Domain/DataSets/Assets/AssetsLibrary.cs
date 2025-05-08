using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsLibrary
{
	IReadOnlyCollection<Image> Images { get; }
	IReadOnlyCollection<Asset> Assets { get; }

	bool Contains(Image image);
	void DeleteAsset(Image image);
}