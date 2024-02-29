namespace SightKeeper.Application.DataSet.Editing;

public interface DataSetEditor
{
    IObservable<Domain.Model.DataSet.DataSet> DataSetEdited { get; }
    Task ApplyChanges(DataSetChangesDTO dataSetChanges, CancellationToken cancellationToken = default);
}