using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class PoserItem : AssetItem
{
	public PoserTag Tag
	{
		get => _tag;
		set
		{
			InappropriateTagsOwnerChangeException.ThrowIfOwnerChanged(_tag, value);
			_tag = value;
		}
	}

	public abstract IReadOnlyCollection<KeyPoint> KeyPoints { get; }

	protected PoserItem(Bounding bounding, PoserTag tag) : base(bounding)
	{
		_tag = tag;
	}

	private PoserTag _tag;
}