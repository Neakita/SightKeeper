using SightKeeper.Domain.Model;

namespace SightKeeper.Application.DataSet.Creating;

public interface NewDataSetInfo : DataSetInfo
{
    ModelType ModelType { get; }
}