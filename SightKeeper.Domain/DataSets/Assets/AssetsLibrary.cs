using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Domain.DataSets.Assets;

public abstract class AssetsLibrary
{
	public abstract bool Contains(Screenshot screenshot);
}