using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using SightKeeper.Application.Scoring;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Annotating.AutoAnnotating;

public sealed partial class AutoAnnotationViewModel : ViewModel, IAutoAnnotationViewModel
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
        get => _detector.ProbabilityThreshold;
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
        get => _detector.IoU;
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
        selectedScreenshotViewModel.NotifyCanExecuteChanged(ClearCommand);
    }
    
    private readonly Detector _detector;
    private readonly SelectedScreenshotViewModel _selectedScreenshotViewModel;
    private readonly SelectedDataSetViewModel _selectedDataSetViewModel;

    ICommand IAutoAnnotationViewModel.AnnotateCommand => AnnotateCommand;
    [RelayCommand(CanExecute = nameof(CanAnnotate))]
    private async Task Annotate(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(_selectedScreenshotViewModel.Value);
        var screenshotContent = _selectedScreenshotViewModel.Value.Image;
        var image = await screenshotContent;
        var items = await _detector.Detect(image.Content, cancellationToken);
        _selectedScreenshotViewModel.DetectedItems.Clear();
        _selectedScreenshotViewModel.DetectedItems.AddRange(items.Select(CreateDetectedItemViewModel));
        ClearCommand.NotifyCanExecuteChanged();
    }
    private bool CanAnnotate() => SelectedWeights != null && _selectedScreenshotViewModel.Value != null && !AutoAnnotatingEnabled;

    ICommand IAutoAnnotationViewModel.ClearCommand => ClearCommand;
    [RelayCommand(CanExecute = nameof(CanClear))]
    private void Clear()
    {
        _selectedScreenshotViewModel.DetectedItems.Clear();
        ClearCommand.NotifyCanExecuteChanged();
    }
    private bool CanClear() => _selectedScreenshotViewModel.DetectedItems.Count > 0;

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
        if (AutoAnnotatingEnabled)
            BeginAnnotationAndForget();
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