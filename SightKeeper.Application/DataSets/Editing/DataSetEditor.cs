using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.DataSets.Editing;

public interface DataSetEditor
{
    IObservable<DataSet> DataSetEdited { get; }
    Task ApplyChangesAsync(DataSetChanges dataSetChanges, CancellationToken cancellationToken = default);
}