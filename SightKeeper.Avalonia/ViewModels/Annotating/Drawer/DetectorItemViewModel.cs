using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class DetectorItemViewModel : ViewModel, DrawerItem
{
    public static IObservable<DetectorItemViewModel> ItemClassChanged => ItemClassChangedSubject.AsObservable();
    private static readonly Subject<DetectorItemViewModel> ItemClassChangedSubject = new();

    public DetectorItem? Item
    {
        get => _item;
        set
        {
            if (_item != null)
                ThrowHelper.ThrowInvalidOperationException("Value already set");
            SetProperty(ref _item, value);
        }
    }

    public DetectorItemResizer Resizer { get; }
    public DrawerViewModel Drawer { get; }

    public bool IsDashed => false;
    public BoundingViewModel Bounding { get; private set; }

    public DetectorItemViewModel(DetectorItem item, DetectorItemResizer resizer, DrawerViewModel drawer)
    {
        Item = item;
        Resizer = resizer;
        Drawer = drawer;
        _itemClass = item.ItemClass;
        Bounding = new BoundingViewModel(item.Bounding);
    }

    public DetectorItemViewModel(ItemClass itemClass, Point position, DetectorItemResizer resizer, DrawerViewModel drawer)
    {
        Resizer = resizer;
        Drawer = drawer;
        _itemClass = itemClass;
        Bounding = new BoundingViewModel(position);
    }

    private DetectorItem? _item;
    [ObservableProperty] private ItemClass _itemClass;
    [ObservableProperty] private bool _isThumbsVisible;

    partial void OnItemClassChanged(ItemClass value) =>
        ItemClassChangedSubject.OnNext(this);
}