using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public abstract class PoserItem : BoundedItem, AssetItem, TagUser
{
	public PoserTag Tag
	{
		get => _tag;
		set
		{
			InappropriateTagsOwnerChangeException.ThrowIfOwnerChanged(_tag, value);
			_tag.RemoveUser(this);
			_tag = value;
			_tag.AddUser(this);
			ClearKeyPoints();
		}
	}

	DomainTag AssetItem.Tag => Tag;

	public abstract IReadOnlyCollection<KeyPoint> KeyPoints { get; }

	public abstract KeyPoint MakeKeyPoint(DomainTag tag);
	public abstract void DeleteKeyPoint(KeyPoint keyPoint);
	public abstract void ClearKeyPoints();

	protected PoserItem(PoserTag tag)
	{
		_tag = tag;
		_tag.AddUser(this);
	}

	private PoserTag _tag;
}