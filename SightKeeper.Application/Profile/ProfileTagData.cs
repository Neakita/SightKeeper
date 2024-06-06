using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Application;

public interface ProfileTagData
{
    public Tag Tag { get; }
    public byte Order { get; }
    public ActivationCondition ActivationCondition { get; }
}