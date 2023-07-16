namespace SightKeeper.Application.Model.Editing;

public interface ModelEditor
{
    public void ApplyChanges(ModelChangesDTO changes);
}