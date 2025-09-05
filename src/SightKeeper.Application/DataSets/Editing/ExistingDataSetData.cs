using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Editing;

public interface ExistingDataSetData : DataSetData
{
	DataSet<Tag, Asset> DataSet { get; }
	TagsChanges TagsChanges { get; }
}