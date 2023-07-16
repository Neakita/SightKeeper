using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Model.Creating;

public interface NewModelData : ModelData
{
    ModelType ModelType { get; }
}