using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface ProfileItemClassData
{
    public ItemClass ItemClass { get; }
    public byte Order { get; }
    public ItemClassActivationCondition ActivationCondition { get; }
}