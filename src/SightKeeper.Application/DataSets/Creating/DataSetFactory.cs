using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.DataSets.Creating;

public interface DataSetFactory<out TTag, out TAsset>
{
	DataSet<TTag, TAsset> CreateDataSet();
}