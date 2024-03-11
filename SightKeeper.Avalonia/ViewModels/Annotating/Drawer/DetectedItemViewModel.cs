using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class DetectedItemViewModel : ViewModel, DrawerItem
{
    public static IObservable<DetectedItemViewModel> MakeAnnotationRequested =>
        MakeAnnotationRequestedSubject.AsObservable();
    private static readonly Subject<DetectedItemViewModel> MakeAnnotationRequestedSubject = new();

    public bool IsDashed => true;
    public BoundingViewModel Bounding { get; }
    public Tag Tag { get; }
    public float Probability { get; }

    public DetectedItemViewModel(BoundingViewModel bounding, Tag tag,  float probability)
    {
        Bounding = bounding;
        Tag = tag;
        Probability = probability;
    }

    [RelayCommand]
    private void MakeAnnotation() => MakeAnnotationRequestedSubject.OnNext(this);
}