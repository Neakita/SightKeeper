using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserItem : DetectorItem
{
	public IReadOnlyList<KeyPoint> KeyPoints { get; }
	
	internal PoserItem(Tag tag, Bounding bounding, IEnumerable<KeyPoint> keyPoints) : base(tag, bounding)
	{
		KeyPoints = keyPoints.ToImmutableArray();
	}
}