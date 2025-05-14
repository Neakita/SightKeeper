using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class Poser2DItem : PoserItem<KeyPoint>
{
	protected override KeyPoint CreateKeyPoint(Tag tag)
	{
		return new KeyPoint(tag);
	}

	internal Poser2DItem(Bounding bounding, PoserTag tag) : base(bounding, tag)
	{
	}
}