using System.Diagnostics;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public abstract class PoserItem<TKeyPoint> : PoserItem where TKeyPoint : KeyPoint
{
	public sealed override IReadOnlyCollection<TKeyPoint> KeyPoints => _keyPoints;

	public override TKeyPoint MakeKeyPoint(Tag tag, Vector2<double> position)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(Tag, tag);
		var keyPoint = CreateKeyPoint(tag, position);
		bool isAdded = _keyPoints.Add(keyPoint);
		Debug.Assert(isAdded);
		tag.AddUser(keyPoint);
		return keyPoint;
	}

	public void DeleteKeyPoint(TKeyPoint keyPoint)
	{
		bool isRemoved = _keyPoints.Remove(keyPoint);
		if (!isRemoved)
			throw new ArgumentException("Specified key point tag was not found and therefore not deleted", nameof(keyPoint));
		keyPoint.Tag.RemoveUser(keyPoint);
	}

	public override void DeleteKeyPoint(KeyPoint keyPoint)
	{
		DeleteKeyPoint((TKeyPoint)keyPoint);
	}

	public override void ClearKeyPoints()
	{
		_keyPoints.Clear();
	}

	protected abstract TKeyPoint CreateKeyPoint(Tag tag, Vector2<double> position);

	internal PoserItem(Bounding bounding, PoserTag tag) : base(bounding, tag)
	{
	}

	private readonly HashSet<TKeyPoint> _keyPoints = new(TagKeyPointEqualityComparer.Instance);
}