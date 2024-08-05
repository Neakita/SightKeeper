namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class KeyPointTag : Tag
{
	public PoserTag PoserTag { get; }

	internal KeyPointTag(string name, PoserTag poserTag) : base(name, poserTag.KeyPoints)
	{
		PoserTag = poserTag;
	}

	protected override IEnumerable<Tag> Siblings => PoserTag.KeyPoints;
}