using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Model;

public interface DataSetData
{
    string Name { get; }
    string Description { get; }
    int? ResolutionWidth { get; }
    int? ResolutionHeight { get; }
    IReadOnlyCollection<string> ItemClasses { get; }
    Game? Game { get; }
}