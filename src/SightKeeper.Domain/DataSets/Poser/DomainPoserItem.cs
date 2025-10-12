using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public sealed class DomainPoserItem(PoserItem inner) : PoserItem, Decorator<PoserItem>
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

	public IReadOnlyCollection<KeyPoint> KeyPoints => inner.KeyPoints;
	public PoserItem Inner => inner;

	public KeyPoint MakeKeyPoint(Tag tag)
	{
		UnexpectedTagsOwnerException.ThrowIfTagsOwnerDoesNotMatch(Tag, tag);
		return inner.MakeKeyPoint(tag);
	}

	public void DeleteKeyPoint(KeyPoint keyPoint)
	{
		inner.DeleteKeyPoint(keyPoint);
	}

	public void ClearKeyPoints()
	{
		inner.ClearKeyPoints();
	}
}