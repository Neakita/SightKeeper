using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Application.Misc;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.DataSets.Creating;

public interface NewDataSetData : DataSetData
{
	Factory<DataSet<Tag, Asset>> DataSetFactory { get; }
	IEnumerable<NewTagData> NewTags { get; }
}