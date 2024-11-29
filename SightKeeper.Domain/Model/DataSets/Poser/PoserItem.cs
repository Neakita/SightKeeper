using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class PoserItem : AssetItem
{
	public abstract IReadOnlyCollection<KeyPoint> KeyPoints { get; }

	protected PoserItem(Bounding bounding) : base(bounding)
	{
	}
}