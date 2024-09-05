using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Application.DataSets;

public interface DataSetData
{
    string Name { get; }
    string Description { get; }
    Composition? Composition { get; }
    Game? Game { get; }
}