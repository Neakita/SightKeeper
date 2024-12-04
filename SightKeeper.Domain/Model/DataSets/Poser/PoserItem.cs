using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public abstract class PoserItem : AssetItem
{
	public PoserTag Tag
	{
		get;
		set
		{
			InappropriateTagsOwnerChangeException.ThrowIfOwnerChanged(field, value);
			field = value;
		}
	}

	public abstract IReadOnlyCollection<KeyPoint> KeyPoints { get; }

	protected PoserItem(Bounding bounding, PoserTag tag) : base(bounding)
	{
		Tag = tag;
	}
}