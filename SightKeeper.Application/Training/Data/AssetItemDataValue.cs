using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Application.Training.Data;

public sealed class AssetItemDataValue : AssetItemData
{
	public TagData Tag { get; }
	public Bounding Bounding { get; }

	public AssetItemDataValue(TagData tag, Bounding bounding)
	{
		Tag = tag;
		Bounding = bounding;
	}
}