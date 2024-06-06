using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Application.DataSets;

public interface DataSetsDataAccess
{
	IObservable<DetectorDataSet> DataSetAdded { get; }
	IObservable<DetectorDataSet> DataSetRemoved { get; }
	IEnumerable<DetectorDataSet> DataSets { get; }

	void AddDataSet(DetectorDataSet dataSet);
	void RemoveDataSet(DetectorDataSet dataSet);
}