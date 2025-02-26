using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public abstract class AssetsLibrary
{
	public abstract bool Contains(Image image);
}