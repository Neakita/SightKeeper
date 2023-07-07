using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Modelling;

public interface ModelChanges
{
    Model Model { get; }
    string Name { get; }
    string Description { get; }
    int ResolutionWidth { get; }
    int ResolutionHeight { get; }
    IReadOnlyCollection<string> ItemClasses { get; }
    Game? Game { get; }
    ModelConfig? Config { get; }
}