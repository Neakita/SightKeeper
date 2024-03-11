using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Application;

public interface ProfileItemClassData
{
    public ItemClass ItemClass { get; }
    public byte Order { get; }
    public ActivationCondition ActivationCondition { get; }
}