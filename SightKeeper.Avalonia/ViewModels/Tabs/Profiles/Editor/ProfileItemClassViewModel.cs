using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public sealed class ProfileItemClassViewModel : ViewModel, ProfileItemClassData
{
    private bool? _whenShooting;
    public ItemClass ItemClass { get; }
    public byte Order { get; set; }

    public ItemClassActivationCondition ActivationCondition => WhenShooting switch
    {
        true => ItemClassActivationCondition.IsShooting,
        false => ItemClassActivationCondition.None,
        null => ItemClassActivationCondition.IsNotShooting
    };

    public bool? WhenShooting
    {
        get => _whenShooting;
        set
        {
            if (SetProperty(ref _whenShooting, value))
                OnPropertyChanged(nameof(ActivationCondition));
        }
    }

    public ProfileItemClassViewModel(ItemClass itemClass, byte order, ItemClassActivationCondition activationCondition = ItemClassActivationCondition.None)
    {
        ItemClass = itemClass;
        Order = order;
        _whenShooting = activationCondition switch
        {
            ItemClassActivationCondition.None => false,
            ItemClassActivationCondition.IsShooting => true,
            ItemClassActivationCondition.IsNotShooting => null,
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<bool>(nameof(activationCondition), activationCondition, null)
        };
    }
}