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
		}
	}

	Tag AssetItem.Tag => Tag;

	public abstract IReadOnlyCollection<KeyPoint> KeyPoints { get; }

	public abstract KeyPoint CreateKeyPoint(Tag tag, Vector2<double> position);
	public abstract void DeleteKeyPoint(KeyPoint keyPoint);

	protected PoserItem(Bounding bounding, PoserTag tag) : base(bounding)
	{
		_tag = tag;
		_tag.AddUser(this);
	}

	private PoserTag _tag;
}