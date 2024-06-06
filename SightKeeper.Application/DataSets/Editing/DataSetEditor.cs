using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Application.DataSets.Editing;

public interface DataSetEditor
{
    IObservable<DetectorDataSet> DataSetEdited { get; }
    Task ApplyChanges(DataSetChangesDTO dataSetChanges, CancellationToken cancellationToken = default);
}