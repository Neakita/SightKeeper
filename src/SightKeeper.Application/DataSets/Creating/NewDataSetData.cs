using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Creating;

public interface NewDataSetData : DataSetData
{
	DataSetFactory<Tag, Asset> DataSetFactory { get; }
	IEnumerable<NewTagData> NewTags { get; }
}