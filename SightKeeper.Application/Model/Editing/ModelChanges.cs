namespace SightKeeper.Application.Model.Editing;

public interface ModelChanges : ModelData
{
    Domain.Model.DataSet DataSet { get; }
}