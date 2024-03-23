using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public sealed class ProfileItemClassViewModel : ViewModel, ProfileItemClassData
{
    public ItemClass ItemClass { get; }
    public byte Order { get; set; }

    public ActivationCondition ActivationCondition
    {
        get => _activationCondition;
        set => SetProperty(ref _activationCondition, value);
    }

    public ProfileItemClassViewModel(ItemClass itemClass, byte order, ActivationCondition activationCondition = ActivationCondition.None)
    {
        ItemClass = itemClass;
        Order = order;
        _activationCondition = activationCondition;
    }
    
    private ActivationCondition _activationCondition;
}