using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsContainer<out TAsset>
{
	IReadOnlyCollection<TAsset> Assets { get; }
	IReadOnlyCollection<Image> Images { get; }
	TAsset? GetOptionalAsset(Image image);
	bool Contains(Image image);
}