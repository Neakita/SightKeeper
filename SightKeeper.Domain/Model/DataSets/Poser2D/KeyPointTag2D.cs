namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class KeyPointTag2D : Tag
{
	public Poser2DTag PoserTag { get; }

	internal KeyPointTag2D(string name, Poser2DTag poserTag) : base(name, poserTag.KeyPoints)
	{
		PoserTag = poserTag;
	}

	protected override IEnumerable<Tag> Siblings => PoserTag.KeyPoints;
}