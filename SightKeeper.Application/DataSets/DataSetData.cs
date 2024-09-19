using SightKeeper.Domain.Model;

namespace SightKeeper.Application.DataSets;

public interface DataSetData
{
    string Name { get; }
    string Description { get; }
    Game? Game { get; }
}