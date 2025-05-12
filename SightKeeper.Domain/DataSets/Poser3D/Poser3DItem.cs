using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class Poser3DItem : PoserItem<KeyPoint3D>
{
	protected override KeyPoint3D CreateKeyPoint(Tag tag, Vector2<double> position)
	{
		return new KeyPoint3D(tag, position);
	}

	internal Poser3DItem(Bounding bounding, PoserTag tag) : base(bounding, tag)
	{
	}
}