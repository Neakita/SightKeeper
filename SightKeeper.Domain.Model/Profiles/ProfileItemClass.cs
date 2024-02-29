using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;
using SightKeeper.Domain.Model.DataSet;

namespace SightKeeper.Domain.Model.Profiles;

public sealed class ProfileItemClass : ObservableObject
{
    public Id Id { get; private set; }
    public ItemClass ItemClass { get; private set; }

    public byte Index
    {
        get => _index;
        internal set => SetProperty(ref _index, value);
    }

    public ItemClassActivationCondition ActivationCondition
    {
        get => _activationCondition;
        set => SetProperty(ref _activationCondition, value);
    }

    public ProfileItemClass(ItemClass itemClass, byte index, ItemClassActivationCondition activationCondition)
    {
        ItemClass = itemClass;
        Index = index;
        ActivationCondition = activationCondition;
    }

    private ProfileItemClass()
    {
        ItemClass = null!;
    }
    
    private byte _index;
    private ItemClassActivationCondition _activationCondition;
}