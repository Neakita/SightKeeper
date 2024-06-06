using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

internal sealed class ProfileTagViewModel : ViewModel, ProfileTagData
{
    public Tag Tag { get; }
    public byte Order { get; set; }

    public ActivationCondition ActivationCondition
    {
        get => _activationCondition;
        set => SetProperty(ref _activationCondition, value);
    }

    public ProfileTagViewModel(Tag tag, byte order, ActivationCondition activationCondition = ActivationCondition.None)
    {
        Tag = tag;
        Order = order;
        _activationCondition = activationCondition;
    }
    
    private ActivationCondition _activationCondition;
}