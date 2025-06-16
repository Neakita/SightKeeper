using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser3D;

public sealed class DomainPoser3DItem(Poser3DItem inner) : Poser3DItem
{
	public Bounding Bounding
	{
		get => inner.Bounding;
		set
		{
			ItemBoundingConstraintException.ThrowIfNotNormalized(this, value);
			inner.Bounding = value;
		}
	}

	public PoserTag Tag
	{
		get => inner.Tag;
		set
		{
			InappropriateTagsOwnerChangeException.ThrowIfOwnerChanged(Tag, value);
			inner.Tag = value;
			ClearKeyPoints();
		}
	}

	public IReadOnlyCollection<KeyPoint3D> KeyPoints => inner.KeyPoints;

	public void DeleteKeyPoint(KeyPoint3D keyPoint)
	{
		inner.DeleteKeyPoint(keyPoint);
	}

	public KeyPoint3D MakeKeyPoint(Tag tag)
	{
		return inner.MakeKeyPoint(tag);
	}

	public void ClearKeyPoints()
	{
		inner.ClearKeyPoints();
	}
}