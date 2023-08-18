using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Model.Creating;

public interface NewDataSetData : DataSetData
{
    ModelType ModelType { get; }
}