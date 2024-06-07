using System.Collections.Immutable;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserItem
{
	public PoserTag Tag { get; set; }
	public Bounding Bounding { get; set; }
	public IReadOnlyList<KeyPoint> KeyPoints { get; }
	
	internal PoserItem(PoserTag tag, Bounding bounding, IEnumerable<KeyPoint> keyPoints)
	{
		Tag = tag;
		Bounding = bounding;
		KeyPoints = keyPoints.ToImmutableArray();
	}
}