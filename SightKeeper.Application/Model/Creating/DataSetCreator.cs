namespace SightKeeper.Application.Model.Creating;

public interface DataSetCreator
{
    IObservable<SightKeeper.Domain.Model.DataSet> ModelCreated { get; }
    Task<Domain.Model.DataSet> CreateDataSet(NewDataSetDataDTO dataSetData, CancellationToken cancellationToken = default);
}