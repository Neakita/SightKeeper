using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class Poser3DItem : PoserItem
{
	public override IReadOnlyCollection<KeyPoint3D> KeyPoints => _keyPoints;

	public override KeyPoint3D CreateKeyPoint(Tag tag, Vector2<double> position)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(Tag, tag);
		KeyPoint3D keyPoint = new(tag, position);
		_keyPoints.Add(keyPoint);
		return keyPoint;
	}

	public KeyPoint3D CreateKeyPoint(Tag tag, Vector2<double> position, bool isVisible)
	{
		var keyPoint = CreateKeyPoint(tag, position);
		keyPoint.IsVisible = isVisible;
		return keyPoint;
	}

	public void DeleteKeyPoint(KeyPoint3D keyPoint)
	{
		bool isRemoved = _keyPoints.Remove(keyPoint);
		if (!isRemoved)
			throw new ArgumentException("Specified key point tag was not found and therefore not deleted", nameof(keyPoint));
	}

	public override void DeleteKeyPoint(KeyPoint keyPoint)
	{
		DeleteKeyPoint((KeyPoint3D)keyPoint);
	}

	internal Poser3DItem(Bounding bounding, PoserTag tag) : base(bounding, tag)
	{
	}

	private readonly HashSet<KeyPoint3D> _keyPoints = new(TagKeyPointEqualityComparer.Instance);
}