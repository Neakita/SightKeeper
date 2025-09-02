using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Creating;

public interface NewDataSetData : DataSetData
{
	DataSetType Type { get; }
	IEnumerable<NewTagData> NewTags { get; }
}