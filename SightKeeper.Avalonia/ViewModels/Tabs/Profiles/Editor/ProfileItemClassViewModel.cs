using SightKeeper.Application;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public sealed class ProfileItemClassViewModel : ViewModel, ProfileItemClassData
{
    public Tag ItemClass { get; }
    public byte Order { get; set; }

    public ItemClassActivationCondition ActivationCondition
    {
        get => _activationCondition;
        set => SetProperty(ref _activationCondition, value);
    }

    public ProfileItemClassViewModel(Tag tag, byte order, ItemClassActivationCondition activationCondition = ItemClassActivationCondition.None)
    {
        ItemClass = tag;
        Order = order;
        _activationCondition = activationCondition;
    }
    
    private ItemClassActivationCondition _activationCondition;
}