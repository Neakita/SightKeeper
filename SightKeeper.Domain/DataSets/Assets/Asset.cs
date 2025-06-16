using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface Asset
{
	public Image Image { get; }
	public AssetUsage Usage { get; set; }
}