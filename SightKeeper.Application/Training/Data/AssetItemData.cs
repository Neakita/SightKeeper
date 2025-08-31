using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training.Data;

public interface AssetItemData
{
	ReadOnlyTag Tag { get; }
	Bounding Bounding { get; }
}