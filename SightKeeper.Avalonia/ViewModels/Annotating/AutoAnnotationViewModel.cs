using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using SightKeeper.Application.Scoring;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AutoAnnotationViewModel : ViewModel
{
    public IReadOnlyCollection<Weights> Weights => _selectedDataSetViewModel.Weights;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AnnotateCommand))]
    private Weights? _selectedWeights;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AnnotateCommand))]
    private bool _autoAnnotatingEnabled;

    public float ProbabilityThreshold
    {
        get => _detector.ProbabilityThreshold ?? 0;
        set
        {
            if (!SetProperty(_detector.ProbabilityThreshold, value, newValue => _detector.ProbabilityThreshold = newValue))
                return;
            if (AutoAnnotatingEnabled)
                BeginAnnotationAndForget();
        }
    }

    public float IoU
    {
        get => _detector.IoU ?? 0;
        set
        {
            if (!SetProperty(_detector.IoU, value, newValue => _detector.IoU = newValue))
                return;
            if (AutoAnnotatingEnabled)
                BeginAnnotationAndForget();
        }
    }

    public AutoAnnotationViewModel(Detector detector, SelectedScreenshotViewModel selectedScreenshotViewModel, SelectedDataSetViewModel selectedDataSetViewModel)
    {
        _detector = detector;
        _selectedScreenshotViewModel = selectedScreenshotViewModel;
        _selectedDataSetViewModel = selectedDataSetViewModel;
        selectedScreenshotViewModel.NotifyCanExecuteChanged(AnnotateCommand);
    }
    
    private readonly Detector _detector;
    private readonly SelectedScreenshotViewModel _selectedScreenshotViewModel;
    private readonly SelectedDataSetViewModel _selectedDataSetViewModel;

    [RelayCommand(CanExecute = nameof(CanAnnotate))]
    private async Task Annotate(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(_selectedScreenshotViewModel.Value);
        var screenshotContent = _selectedScreenshotViewModel.Value.Content;
        var content = await screenshotContent;
        var items = await _detector.Detect(content, cancellationToken);
        _selectedScreenshotViewModel.DetectedItems.Clear();
        _selectedScreenshotViewModel.DetectedItems.AddRange(items.Select(CreateDetectedItemViewModel));
    }
    private bool CanAnnotate() => SelectedWeights != null && _selectedScreenshotViewModel.Value != null && !AutoAnnotatingEnabled;

    private static DetectedItemViewModel CreateDetectedItemViewModel(DetectionItem detectionItem)
    {
        var rect = detectionItem.Bounding;
        Bounding bounding = new(rect.Left, rect.Top, rect.Right, rect.Bottom);
        BoundingViewModel boundingViewModel = new(bounding);
        return new DetectedItemViewModel(boundingViewModel, detectionItem.ItemClass, detectionItem.Probability);
    }

    partial void OnSelectedWeightsChanged(Weights? value)
    {
        if (value == null)
            AutoAnnotatingEnabled = false;
        _detector.Weights = value;
        OnPropertyChanged(nameof(ProbabilityThreshold));
        OnPropertyChanged(nameof(IoU));
    }

    private IDisposable? _isAutoDetectedEnabledChangedSubscription;
    private CancellationTokenSource? _autoDetectCancellationTokenSource;
    partial void OnAutoAnnotatingEnabledChanged(bool value)
    {
        if (value)
        {
            Guard.IsNull(_isAutoDetectedEnabledChangedSubscription);
            _isAutoDetectedEnabledChangedSubscription =
                _selectedScreenshotViewModel.ObservableValue.Subscribe(_ => BeginAnnotationAndForget());
            BeginAnnotationAndForget();
        }
        else
        {
            Guard.IsNotNull(_isAutoDetectedEnabledChangedSubscription);
            _isAutoDetectedEnabledChangedSubscription.Dispose();
            _isAutoDetectedEnabledChangedSubscription = null;
        }
    }

    private void BeginAnnotationAndForget()
    {
        _autoDetectCancellationTokenSource?.Cancel();
        _autoDetectCancellationTokenSource = new CancellationTokenSource();
        _ = Annotate(_autoDetectCancellationTokenSource.Token);
    }
}