using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface Asset
{
	Image Image { get; }
	AssetUsage Usage { get; set; }
}