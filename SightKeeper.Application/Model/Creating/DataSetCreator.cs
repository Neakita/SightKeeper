namespace SightKeeper.Application.Model.Creating;

public interface DataSetCreator
{
    IObservable<SightKeeper.Domain.Model.DataSet> ModelCreated { get; }
    Task<Domain.Model.DataSet> CreateDataSet(NewDataSetDataSetDataSetDataDTO dataSetDataSetDataSetData, CancellationToken cancellationToken = default);
}