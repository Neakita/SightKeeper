using System.Collections.Immutable;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DItem
{
	public Poser3DTag Tag { get; set; }
	public Bounding Bounding { get; set; }
	public IReadOnlyList<KeyPoint3D> KeyPoints { get; }
	
	internal Poser3DItem(Poser3DTag tag, Bounding bounding, IEnumerable<KeyPoint3D> keyPoints)
	{
		Tag = tag;
		Bounding = bounding;
		KeyPoints = keyPoints.ToImmutableList();
	}
}