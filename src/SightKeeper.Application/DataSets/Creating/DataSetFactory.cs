using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.DataSets.Creating;

public interface DataSetFactory<out TAsset>
{
	DataSet<TAsset> CreateDataSet();
}