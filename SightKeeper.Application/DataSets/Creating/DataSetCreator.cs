using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.DataSets.Creating;

public interface DataSetCreator
{
    IObservable<DataSet> DataSetCreated { get; }
    DataSet CreateDataSet(NewDataSetInfoDTO newDataSetInfo);
}