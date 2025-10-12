using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using Vibrance.Changes;

namespace SightKeeper.Data.DataSets.Poser.Items.Decorators;

internal sealed class ObservableKeyPointsPoserItem(PoserItem inner) : PoserItem, Decorator<PoserItem>
{
	public Bounding Bounding
	{
		get => inner.Bounding;
		set => inner.Bounding = value;
	}

	public PoserTag Tag
	{
		get => inner.Tag;
		set => inner.Tag = value;
	}

	public IReadOnlyList<KeyPoint> KeyPoints => _keyPoints;
	public PoserItem Inner => inner;

	public KeyPoint MakeKeyPoint(Tag tag)
	{
		var index = KeyPoints.Count;
		var keyPoint = inner.MakeKeyPoint(tag);
		if (_keyPoints.HasObservers)
		{
			var change = new Insertion<KeyPoint>
			{
				Items = [keyPoint],
				Index = index
			};
			_keyPoints.Notify(change);
		}
		return keyPoint;
	}

	public void DeleteKeyPoint(KeyPoint keyPoint)
	{
		var keyPointIndex = KeyPoints.Index().Single(x => x.Item == keyPoint).Index;
		inner.DeleteKeyPoint(keyPoint);
		if (!_keyPoints.HasObservers)
			return;
		var change = new IndexedRemoval<KeyPoint>
		{
			Items = [keyPoint],
			Index = keyPointIndex
		};
		_keyPoints.Notify(change);
	}

	public void ClearKeyPoints()
	{
		var oldKeyPoints = inner.KeyPoints.ToList();
		inner.ClearKeyPoints();
		if (!_keyPoints.HasObservers || oldKeyPoints.Count == 0)
			return;
		var change = new Reset<KeyPoint>
		{
			OldItems = oldKeyPoints
		};
		_keyPoints.Notify(change);
	}

	private readonly ExternalObservableList<KeyPoint> _keyPoints = new(inner.KeyPoints);
}