namespace SightKeeper.Application.Model.Editing;

public interface DataSetEditor
{
    IObservable<Domain.Model.DataSet> ModelEdited { get; }
    Task ApplyChanges(DataSetChangesDTO changes, CancellationToken cancellationToken = default);
}