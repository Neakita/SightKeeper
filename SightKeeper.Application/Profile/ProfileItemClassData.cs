using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Application;

public interface ProfileItemClassData
{
    public ItemClass ItemClass { get; }
    public byte Order { get; }
    public ItemClassActivationCondition ActivationCondition { get; }
}