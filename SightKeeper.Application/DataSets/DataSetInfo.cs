using SightKeeper.Domain.Model;

namespace SightKeeper.Application.DataSets;

public interface DataSetInfo
{
    string Name { get; }
    string Description { get; }
    IReadOnlyCollection<ItemClassInfo> ItemClasses { get; }
    Game? Game { get; }
}