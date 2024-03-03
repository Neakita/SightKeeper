using SightKeeper.Domain.Model;

namespace SightKeeper.Application.DataSets.Creating;

public interface DataSetCreator
{
    IObservable<DataSet> DataSetCreated { get; }
    Task<DataSet> CreateDataSet(NewDataSetInfoDTO newDataSetInfo, CancellationToken cancellationToken = default);
}