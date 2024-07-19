using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

internal abstract partial class AbstractProfileEditorViewModel<TProfileData> : DialogViewModel<ProfileEditorResult>, INotifyDataErrorInfo, ProfileEditorViewModel where TProfileData : class, ProfileData
{
	protected readonly ViewModelValidator<TProfileData> Validator;
    public IReadOnlyCollection<DetectorDataSet> AvailableDataSets { get; }

    public IReadOnlyCollection<Weights> AvailableWeights
    {
        get => _availableWeights;
        private set => SetProperty(ref _availableWeights, value);
    }

    public IReadOnlyCollection<Tag> AvailableTags { get; }
    
    public Profile? Profile { get; protected set; }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public float DetectionThreshold
    {
        get => _detectionThreshold;
        set => SetProperty(ref _detectionThreshold, value);
    }

    public float MouseSensitivity
    {
        get => _mouseSensitivity;
        set => SetProperty(ref _mouseSensitivity, value);
    }

    public TimeSpan PostProcessDelay
    {
        get => _postProcessDelay;
        set => SetProperty(ref _postProcessDelay, value);
    }

    public bool IsPreemptionEnabled
    {
        get => _isPreemptionEnabled;
        set => SetProperty(ref _isPreemptionEnabled, value);
    }

    public float? PreemptionHorizontalFactor
    {
        get => _preemptionHorizontalFactor;
        set
        {
            if (SetProperty(ref _preemptionHorizontalFactor, value) && PreemptionFactorsLink)
            {
                OnPropertyChanging(nameof(PreemptionVerticalFactor));
                _preemptionVerticalFactor = value;
                OnPropertyChanged(nameof(PreemptionVerticalFactor));
            }
        }
    }

    public float? PreemptionVerticalFactor
    {
        get => _preemptionVerticalFactor;
        set
        {
            if (SetProperty(ref _preemptionVerticalFactor, value) && PreemptionFactorsLink)
            {
                OnPropertyChanging(nameof(PreemptionHorizontalFactor));
                _preemptionHorizontalFactor = value;
                OnPropertyChanged(nameof(PreemptionHorizontalFactor));
            }
        }
    }

    public bool PreemptionFactorsLink
    {
        get => _preemptionFactorsLink;
        set
        {
            if (SetProperty(ref _preemptionFactorsLink, value) && value)
                PreemptionVerticalFactor = PreemptionHorizontalFactor;
        }
    }

    public bool IsPreemptionStabilizationEnabled
    {
        get => _isPreemptionStabilizationEnabled;
        set => SetProperty(ref _isPreemptionStabilizationEnabled, value);
    }

    public byte? PreemptionStabilizationBufferSize
    {
        get => _preemptionStabilizationBufferSize;
        set => SetProperty(ref _preemptionStabilizationBufferSize, value);
    }

    // public StabilizationMethod? PreemptionStabilizationMethod
    // {
    //     get => _preemptionStabilizationMethod;
    //     set => SetProperty(ref _preemptionStabilizationMethod, value);
    // }

    ushort ProfileEditorViewModel.PostProcessDelay
    {
        get => (ushort)_postProcessDelay.TotalMilliseconds;
        set => SetProperty(PostProcessDelay.TotalMilliseconds, value, newValue => _postProcessDelay = TimeSpan.FromMilliseconds(newValue));
    }

    public DetectorDataSet? DataSet
    {
        get => _dataSet;
        set
        {
            if (!SetProperty(ref _dataSet, value))
                return;
            _availableTags.Clear();
            TagsSource.Clear();
            if (value == null)
                AvailableWeights = Array.Empty<Weights>();
            else
            {
                AvailableWeights = value.Weights;
                _availableTags.AddRange(value.Tags);
            }
        }
    }

    public Weights? Weights
    {
        get => _weights;
        set => SetProperty(ref _weights, value);
    }

    public IReadOnlyList<ProfileTagData> Tags { get; }
    IReadOnlyList<ProfileTagViewModel> ProfileEditorViewModel.Tags => _tagsViewModels;
    private readonly ReadOnlyObservableCollection<ProfileTagViewModel> _tagsViewModels;

    protected AbstractProfileEditorViewModel(IValidator<TProfileData> validator, DataSetsObservableRepository dataSetsObservableRepository, bool canDelete)
    {
        _canDelete = canDelete;
        AvailableDataSets = dataSetsObservableRepository.DataSets;
        TagsSource.Connect()
            .Bind(out var tags)
            .Subscribe()
            .DisposeWith(_constructorDisposables);
        Tags = tags;
        _tagsViewModels = tags;
        _availableTags.Connect()
            .Except(TagsSource.Connect().Transform(tagData => tagData.Tag))
            .Bind(out var availableTags)
            .Subscribe()
            .DisposeWith(_constructorDisposables);
        AvailableTags = availableTags;
        var validatable = this as TProfileData;
        Guard.IsNotNull(validatable);
        Validator = new ViewModelValidator<TProfileData>(validator, this, validatable);
    }

    private readonly CompositeDisposable _constructorDisposables = new();
    protected readonly SourceList<ProfileTagViewModel> TagsSource = new();
    private readonly SourceList<Tag> _availableTags = new();
    private readonly bool _canDelete;
    private DetectorDataSet? _dataSet;
    private Weights? _weights;
    private float _mouseSensitivity = 1;
    private float _detectionThreshold = 0.6f;
    private string _description = string.Empty;
    private string _name = string.Empty;
    private IReadOnlyCollection<Weights> _availableWeights = Array.Empty<Weights>();
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddTagCommand))]
    private Tag? _tagToAdd;
    private TimeSpan _postProcessDelay;
    private bool _isPreemptionEnabled;
    private float? _preemptionHorizontalFactor;
    private float? _preemptionVerticalFactor;
    private bool _isPreemptionStabilizationEnabled;
    private byte? _preemptionStabilizationBufferSize;
    // private StabilizationMethod? _preemptionStabilizationMethod;
    private bool _preemptionFactorsLink;

    ICommand ProfileEditorViewModel.AddTagCommand => AddTagCommand;
    [RelayCommand(CanExecute = nameof(CanAddTag))]
    private void AddTag()
    {
        Guard.IsNotNull(TagToAdd);
        TagsSource.Add(new ProfileTagViewModel(TagToAdd, (byte)TagsSource.Count));
        MoveTagUpCommand.NotifyCanExecuteChanged();
        MoveTagDownCommand.NotifyCanExecuteChanged();
    }
    private bool CanAddTag() => TagToAdd != null;

    ICommand ProfileEditorViewModel.RemoveTagCommand => RemoveTagCommand;
    [RelayCommand]
    private void RemoveTag(ProfileTagViewModel tag)
    {
        Guard.IsTrue(TagsSource.Remove(tag));
        UpdateTagsOrder();
        MoveTagUpCommand.NotifyCanExecuteChanged();
        MoveTagDownCommand.NotifyCanExecuteChanged();
    }

    ICommand ProfileEditorViewModel.MoveTagUpCommand => MoveTagUpCommand;
    [RelayCommand(CanExecute = nameof(CanMoveTagUp))]
    private void MoveTagUp(ProfileTagViewModel tag)
    {
        var currentIndex = Tags.IndexOf(tag);
        TagsSource.Move(currentIndex, currentIndex - 1);
        UpdateTagsOrder();
        MoveTagUpCommand.NotifyCanExecuteChanged();
        MoveTagDownCommand.NotifyCanExecuteChanged();
    }
    private bool CanMoveTagUp(ProfileTagViewModel tag)
    {
        var currentIndex = Tags.IndexOf(tag);
        return currentIndex > 0;
    }

    ICommand ProfileEditorViewModel.MoveTagDownCommand => MoveTagDownCommand;
    [RelayCommand(CanExecute = nameof(CanMoveTagDown))]
    private void MoveTagDown(ProfileTagViewModel tag)
    {
        var currentIndex = Tags.IndexOf(tag);
        TagsSource.Move(currentIndex, currentIndex + 1);
        UpdateTagsOrder();
        MoveTagUpCommand.NotifyCanExecuteChanged();
        MoveTagDownCommand.NotifyCanExecuteChanged();
    }
    private bool CanMoveTagDown(ProfileTagViewModel tag)
    {
        var currentIndex = Tags.IndexOf(tag);
        return currentIndex < Tags.Count - 1;
    }

    ICommand ProfileEditorViewModel.ApplyCommand => ApplyCommand;
    [RelayCommand(CanExecute = nameof(CanApply))]
    private void Apply()
    {
	    Guard.IsFalse(HasErrors);
	    Return(ProfileEditorResult.Apply);
    }
    private bool CanApply() => !HasErrors;

    ICommand ProfileEditorViewModel.DeleteCommand => DeleteCommand;

    [RelayCommand(CanExecute = nameof(CanDelete))]
    private void Delete()
    {
        Return(ProfileEditorResult.Delete);
    }
    private bool CanDelete() => _canDelete;

    private void UpdateTagsOrder()
    {
        for (var i = 0; i < Tags.Count; i++)
            TagsSource.Items.ElementAt(i).Order = (byte)i;
    }

    public IEnumerable GetErrors(string? propertyName)
    {
	    return Validator.GetErrors(propertyName);
    }

    public bool HasErrors => Validator.HasErrors;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
    {
	    add => Validator.ErrorsChanged += value;
	    remove => Validator.ErrorsChanged -= value;
    }
}