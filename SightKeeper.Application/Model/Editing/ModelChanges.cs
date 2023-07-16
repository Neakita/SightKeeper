namespace SightKeeper.Application.Model.Editing;

public interface ModelChanges : ModelData
{
    Domain.Model.Model Model { get; }
}