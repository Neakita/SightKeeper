using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class Poser2DItem : PoserItem
{
	public override IReadOnlyCollection<KeyPoint> KeyPoints => _keyPoints;

	public KeyPoint CreateKeyPoint(Tag tag, Vector2<double> position)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(Tag, tag);
		KeyPoint keyPoint = new(tag, position);
		bool isAdded = _keyPoints.Add(keyPoint);
		Guard.IsTrue(isAdded);
		return keyPoint;
	}

	public void DeleteKeyPoint(KeyPoint tag)
	{
		bool isRemoved = _keyPoints.Remove(tag);
		Guard.IsTrue(isRemoved);
	}

	internal Poser2DItem(Bounding bounding, PoserTag tag) : base(bounding, tag)
	{
	}

	private readonly HashSet<KeyPoint> _keyPoints = new(TagKeyPointEqualityComparer.Instance);
}