using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Application.DataSets.Creating;

public interface DataSetCreator
{
    IObservable<DetectorDataSet> DataSetCreated { get; }
    DetectorDataSet CreateDataSet(NewDataSetInfoDTO newDataSetInfo);
}