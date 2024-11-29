using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class KeyPointTag3D : KeyPointTag<Poser3DTag>
{
	public override Poser3DTag PoserTag { get; }

	public override void Delete()
	{
		PoserTag.DeleteKeyPoint(this);
	}

	internal KeyPointTag3D(string name, int index, Poser3DTag poserTag) : base(name, poserTag.KeyPoints, index)
	{
		PoserTag = poserTag;
	}

	protected override IEnumerable<Tag> Siblings => PoserTag.KeyPoints;
}