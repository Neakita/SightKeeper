using CommunityToolkit.Diagnostics;
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

	public KeyPointTag2D(string name, int index, Poser2DTag poserTag) : base(name, poserTag.KeyPoints, index)
	{
		PoserTag = poserTag;
	}

	internal IReadOnlyCollection<KeyPoint2D> KeyPoints => _keyPoints;

	internal void AddKeyPoint(KeyPoint2D keyPoint)
	{
		Guard.IsReferenceEqualTo(keyPoint.Tag, this);
		bool isAdded = _keyPoints.Add(keyPoint);
		Guard.IsTrue(isAdded);
	}

	internal void RemoveKeyPoint(KeyPoint2D keyPoint)
	{
		Guard.IsReferenceEqualTo(keyPoint.Tag, this);
		bool isRemoved = _keyPoints.Remove(keyPoint);
		Guard.IsTrue(isRemoved);
	}

	protected override IEnumerable<Tag> Siblings => PoserTag.KeyPoints;
	private readonly HashSet<KeyPoint2D> _keyPoints = new();
}