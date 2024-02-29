namespace SightKeeper.Application.DataSet.Creating;

public interface DataSetCreator
{
    IObservable<Domain.Model.DataSet.DataSet> DataSetCreated { get; }
    Task<Domain.Model.DataSet.DataSet> CreateDataSet(NewDataSetInfoDTO newDataSetInfo, CancellationToken cancellationToken = default);
}