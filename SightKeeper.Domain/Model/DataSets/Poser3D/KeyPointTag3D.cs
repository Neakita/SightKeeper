namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class KeyPointTag3D : Tag
{
	public Poser3DTag PoserTag { get; }

	internal KeyPointTag3D(string name, Poser3DTag poserTag) : base(name, poserTag.KeyPoints)
	{
		PoserTag = poserTag;
	}

	protected override IEnumerable<Tag> Siblings => PoserTag.KeyPoints;
}