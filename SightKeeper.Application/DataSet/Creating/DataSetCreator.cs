namespace SightKeeper.Application.DataSet.Creating;

public interface DataSetCreator
{
    IObservable<SightKeeper.Domain.Model.DataSet> DataSetCreated { get; }
    Task<Domain.Model.DataSet> CreateDataSet(NewDataSetInfoDTO newDataSetInfo, CancellationToken cancellationToken = default);
}