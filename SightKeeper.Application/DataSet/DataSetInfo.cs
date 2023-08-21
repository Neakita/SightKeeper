using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.DataSet;

public interface DataSetInfo
{
    string Name { get; }
    string Description { get; }
    int? ResolutionWidth { get; }
    int? ResolutionHeight { get; }
    IReadOnlyCollection<string> ItemClasses { get; }
    Game? Game { get; }
}