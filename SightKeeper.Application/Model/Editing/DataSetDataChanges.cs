namespace SightKeeper.Application.Model.Editing;

public interface DataSetDataChanges : DataSetData
{
    Domain.Model.DataSet DataSet { get; }
}