using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Application.DataSets.Creating;

public interface DataSetCreator
{
    DetectorDataSet CreateDataSet(NewDataSetInfoDTO newDataSetInfo);
}