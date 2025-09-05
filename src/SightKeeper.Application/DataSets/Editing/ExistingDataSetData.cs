using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.DataSets.Editing;

public interface ExistingDataSetData : DataSetData
{
	DataSet<Asset> DataSet { get; }
	TagsChanges TagsChanges { get; }
}