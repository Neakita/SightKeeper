using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.DataSets.Editing;

public interface ExistingDataSetData : DataSetData
{
	DataSet DataSet { get; }
	TagsChanges TagsChanges { get; }
}