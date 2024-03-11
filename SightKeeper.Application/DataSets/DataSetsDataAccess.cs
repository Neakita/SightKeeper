using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.DataSets;

public interface DataSetsDataAccess
{
	IObservable<DataSet> DataSetAdded { get; }
	IObservable<DataSet> DataSetRemoved { get; }
	IEnumerable<DataSet> DataSets { get; }

	void AddDataSet(DataSet dataSet);
	void RemoveDataSet(DataSet dataSet);
}