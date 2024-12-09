using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class Poser3DItem : PoserItem
{
	public override IReadOnlyCollection<KeyPoint3D> KeyPoints => _keyPoints;

	public KeyPoint3D CreateKeyPoint(Tag tag, Vector2<double> position, bool isVisible = true)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(Tag, tag);
		KeyPoint3D keyPoint = new(tag, position, isVisible);
		_keyPoints.Add(keyPoint);
		return keyPoint;
	}

	internal Poser3DItem(Bounding bounding, PoserTag tag) : base(bounding, tag)
	{
	}

	private readonly HashSet<KeyPoint3D> _keyPoints = new(TagKeyPointEqualityComparer.Instance);
}