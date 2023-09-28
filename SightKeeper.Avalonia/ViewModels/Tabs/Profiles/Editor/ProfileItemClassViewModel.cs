using SightKeeper.Application;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public sealed class ProfileItemClassViewModel : ViewModel, ProfileItemClassData
{
    public ItemClass ItemClass { get; }
    public byte Order { get; set; }

    public ItemClassActivationCondition ActivationCondition
    {
        get => _activationCondition;
        set => SetProperty(ref _activationCondition, value);
    }

    public ProfileItemClassViewModel(ItemClass itemClass, byte order, ItemClassActivationCondition activationCondition = ItemClassActivationCondition.None)
    {
        ItemClass = itemClass;
        Order = order;
        _activationCondition = activationCondition;
    }
    
    private ItemClassActivationCondition _activationCondition;
}