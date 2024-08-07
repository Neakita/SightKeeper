using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Application;

public interface ProfileTagData
{
    public Tag Tag { get; }
    public byte Order { get; }
    // public ActivationCondition ActivationCondition { get; }
}