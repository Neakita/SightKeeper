namespace SightKeeper.Application.DataSet.Editing;

public interface DataSetChanges : DataSetInfo
{
    Domain.Model.DataSet DataSet { get; }
}