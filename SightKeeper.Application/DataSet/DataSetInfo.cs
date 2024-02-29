using SightKeeper.Domain.Model;

namespace SightKeeper.Application.DataSet;

public interface DataSetInfo
{
    string Name { get; }
    string Description { get; }
    int? Resolution { get; }
    IReadOnlyCollection<ItemClassInfo> ItemClasses { get; }
    Game? Game { get; }
}