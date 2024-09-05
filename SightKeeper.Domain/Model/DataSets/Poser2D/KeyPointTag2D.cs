using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class KeyPointTag2D : KeyPointTag<Poser2DTag>
{
	public override Poser2DTag PoserTag { get; }

	public override void Delete()
	{
		PoserTag.DeleteKeyPoint(this);
	}

	internal KeyPointTag2D(string name, Poser2DTag poserTag) : base(name, poserTag.KeyPoints)
	{
		PoserTag = poserTag;
	}

	protected override IEnumerable<Tag> Siblings => PoserTag.KeyPoints;
}