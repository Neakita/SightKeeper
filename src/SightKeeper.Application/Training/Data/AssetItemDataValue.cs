using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training.Data;

internal sealed class AssetItemDataValue : ReadOnlyAssetItem
{
	public ReadOnlyTag Tag { get; }
	public Bounding Bounding { get; }

	public AssetItemDataValue(ReadOnlyTag tag, Bounding bounding)
	{
		Tag = tag;
		Bounding = bounding;
	}
}