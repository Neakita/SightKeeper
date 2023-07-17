namespace SightKeeper.Application.Model.Editing;

public interface ModelEditor
{
    public Task ApplyChanges(ModelChangesDTO changes, CancellationToken cancellationToken = default);
}