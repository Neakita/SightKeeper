using System.ComponentModel;

namespace SightKeeper.Domain.Model.Profiles;

public enum ItemClassActivationCondition
{
    [Description("None")]
    None,
    [Description("When shooting")]
    IsShooting,
    [Description("When not shooting")]
    IsNotShooting
}