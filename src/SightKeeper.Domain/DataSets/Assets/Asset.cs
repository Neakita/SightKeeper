using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface Asset
{
	ManagedImage Image { get; }
	AssetUsage Usage { get; set; }
}