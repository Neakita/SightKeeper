using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsContainer<out TAsset>
{
	IReadOnlyCollection<TAsset> Assets { get; }
	IReadOnlyCollection<ManagedImage> Images { get; }
	TAsset GetAsset(ManagedImage image);
	TAsset? GetOptionalAsset(ManagedImage image);
	bool Contains(ManagedImage image);
}