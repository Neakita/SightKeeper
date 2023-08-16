namespace SightKeeper.Application.Model.Editing;

public interface ModelEditor
{
    IObservable<Domain.Model.DataSet> ModelEdited { get; }
    Task ApplyChanges(ModelChangesDTO changes, CancellationToken cancellationToken = default);
}