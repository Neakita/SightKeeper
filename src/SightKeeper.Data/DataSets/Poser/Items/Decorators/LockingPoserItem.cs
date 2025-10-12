using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items.Decorators;

internal sealed class LockingPoserItem(PoserItem inner, Lock editingLock) : PoserItem, Decorator<PoserItem>
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

	public IReadOnlyCollection<KeyPoint> KeyPoints => inner.KeyPoints;
	public PoserItem Inner => inner;

	public KeyPoint MakeKeyPoint(Tag tag)
	{
		lock (editingLock)
			return inner.MakeKeyPoint(tag);
	}

	public void DeleteKeyPoint(KeyPoint keyPoint)
	{
		lock (editingLock)
			inner.DeleteKeyPoint(keyPoint);
	}

	public void ClearKeyPoints()
	{
		lock (editingLock)
			inner.ClearKeyPoints();
	}
}