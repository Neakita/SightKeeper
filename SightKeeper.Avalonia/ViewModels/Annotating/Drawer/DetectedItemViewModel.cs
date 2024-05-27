using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.ViewModels.Annotating.Drawer;

internal sealed partial class DetectedItemViewModel : ViewModel, DrawerItem
{
    public static IObservable<DetectedItemViewModel> MakeAnnotationRequested =>
        MakeAnnotationRequestedSubject.AsObservable();
    private static readonly Subject<DetectedItemViewModel> MakeAnnotationRequestedSubject = new();

    public bool IsDashed => true;
    public BoundingViewModel Bounding { get; }
    public ItemClass ItemClass { get; }
    public float Probability { get; }

    public DetectedItemViewModel(BoundingViewModel bounding, ItemClass itemClass,  float probability)
    {
        Bounding = bounding;
        ItemClass = itemClass;
        Probability = probability;
    }

    [RelayCommand]
    private void MakeAnnotation() => MakeAnnotationRequestedSubject.OnNext(this);
}