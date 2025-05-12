using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class Poser2DItem : PoserItem<KeyPoint>
{
	protected override KeyPoint CreateKeyPoint(Tag tag, Vector2<double> position)
	{
		return new KeyPoint(tag, position);
	}

	internal Poser2DItem(Bounding bounding, PoserTag tag) : base(bounding, tag)
	{
	}
}