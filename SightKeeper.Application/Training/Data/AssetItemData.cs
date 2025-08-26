using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Application.Training.Data;

public interface AssetItemData
{
	TagData Tag { get; }
	Bounding Bounding { get; }
}