using System.Diagnostics;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser2D;

public sealed class Poser2DItem : PoserItem
{
	public override IReadOnlyCollection<KeyPoint> KeyPoints => _keyPoints;

	public override KeyPoint CreateKeyPoint(Tag tag, Vector2<double> position)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(Tag, tag);
		KeyPoint keyPoint = new(tag, position);
		bool isAdded = _keyPoints.Add(keyPoint);
		Debug.Assert(isAdded);
		tag.AddUser(keyPoint);
		return keyPoint;
	}

	public override void DeleteKeyPoint(KeyPoint keyPoint)
	{
		bool isRemoved = _keyPoints.Remove(keyPoint);
		if (!isRemoved)
			throw new ArgumentException("Specified key point tag was not found and therefore not deleted", nameof(keyPoint));
		keyPoint.Tag.RemoveUser(keyPoint);
	}

	public override void ClearKeyPoints()
	{
		_keyPoints.Clear();
	}

	internal Poser2DItem(Bounding bounding, PoserTag tag) : base(bounding, tag)
	{
	}

	private readonly HashSet<KeyPoint> _keyPoints = new(TagKeyPointEqualityComparer.Instance);
}