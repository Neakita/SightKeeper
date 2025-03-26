using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsLibrary
{
	IReadOnlyCollection<Image> Images { get; }

	bool Contains(Image image);
}