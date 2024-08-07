using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Avalonia;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Avalonia.ViewModels.Annotating.Drawer;

internal sealed partial class DetectorItemViewModel : ViewModel, DrawerItem
{
    public static IObservable<DetectorItemViewModel> TagChanged => TagChangedSubject.AsObservable();
    private static readonly Subject<DetectorItemViewModel> TagChangedSubject = new();

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
        _tag = item.Tag;
        Bounding = new BoundingViewModel(item.Bounding);
    }

    public DetectorItemViewModel(Tag tag, Point position, DetectorItemResizer resizer, DrawerViewModel drawer)
    {
        Resizer = resizer;
        Drawer = drawer;
        _tag = tag;
        Bounding = new BoundingViewModel(position);
    }

    private DetectorItem? _item;
    [ObservableProperty] private Tag _tag;
    [ObservableProperty] private bool _isThumbsVisible;

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnTagChanged(Tag value) =>
        TagChangedSubject.OnNext(this);
}