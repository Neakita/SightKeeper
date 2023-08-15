using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class DetectorItemViewModel : ViewModel
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
    public DetectorDrawerViewModel Drawer { get; }

    public BoundingBoxViewModel Bounding { get; private set; }

    public DetectorItemViewModel(DetectorItem item, DetectorItemResizer resizer, DetectorDrawerViewModel drawer)
    {
        Item = item;
        Resizer = resizer;
        Drawer = drawer;
        _itemClass = item.ItemClass;
        Bounding = new BoundingBoxViewModel(item.BoundingBox);
    }

    public DetectorItemViewModel(ItemClass itemClass, Point position, DetectorItemResizer resizer, DetectorDrawerViewModel drawer)
    {
        Resizer = resizer;
        Drawer = drawer;
        _itemClass = itemClass;
        Bounding = new BoundingBoxViewModel(position);
    }

    private DetectorItem? _item;
    [ObservableProperty] private ItemClass _itemClass;
    [ObservableProperty] private bool _isThumbsVisible;

    partial void OnItemClassChanged(ItemClass value) =>
        ItemClassChangedSubject.OnNext(this);
}