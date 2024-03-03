using SightKeeper.Domain.Model;

namespace SightKeeper.Application.DataSets.Editing;

public interface DataSetEditor
{
    IObservable<DataSet> DataSetEdited { get; }
    Task ApplyChanges(DataSetChangesDTO dataSetChanges, CancellationToken cancellationToken = default);
}