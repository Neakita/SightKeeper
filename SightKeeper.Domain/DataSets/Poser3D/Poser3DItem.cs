using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class Poser3DItem : PoserItem<KeyPoint3D>
{
	protected override KeyPoint3D CreateKeyPoint(Tag tag)
	{
		return new KeyPoint3D(tag);
	}

	internal Poser3DItem(PoserTag tag) : base(tag)
	{
	}
}