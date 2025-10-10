using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Poser.Items;

internal sealed class TrackablePoserItem(PoserItem inner, ChangeListener listener) : PoserItem
{
	public Bounding Bounding
	{
		get => inner.Bounding;
		set
		{
			inner.Bounding = value;
			listener.SetDataChanged();
		}
	}

	public PoserTag Tag
	{
		get => inner.Tag;
		set
		{
			inner.Tag = value;
			listener.SetDataChanged();
		}
	}

	public IReadOnlyCollection<KeyPoint> KeyPoints => inner.KeyPoints;

	public KeyPoint MakeKeyPoint(Tag tag)
	{
		var keyPoint = inner.MakeKeyPoint(tag);
		listener.SetDataChanged();
		return keyPoint;
	}

	public void DeleteKeyPoint(KeyPoint keyPoint)
	{
		inner.DeleteKeyPoint(keyPoint);
		listener.SetDataChanged();
	}

	public void ClearKeyPoints()
	{
		inner.ClearKeyPoints();
		listener.SetDataChanged();
	}
}