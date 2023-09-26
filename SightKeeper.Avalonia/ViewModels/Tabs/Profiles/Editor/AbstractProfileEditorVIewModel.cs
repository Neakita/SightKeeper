using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Avalonia.ViewModels.Dialogs.Abstract;
using SightKeeper.Commons;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public abstract partial class AbstractProfileEditorVIewModel<TProfileData> : ValidatableDialogViewModel<TProfileData, ProfileEditorResult>, ProfileEditorViewModel where TProfileData : class, ProfileData
{
    public IReadOnlyCollection<DataSet> AvailableDataSets { get; }

    public IReadOnlyCollection<Weights> AvailableWeights
    {
        get => _availableWeights;
        private set => SetProperty(ref _availableWeights, value);
    }

    public IReadOnlyCollection<ItemClass> AvailableItemClasses { get; }
    
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

    ushort ProfileEditorViewModel.PostProcessDelay
    {
        get => (ushort)_postProcessDelay.TotalMilliseconds;
        set => SetProperty(PostProcessDelay.TotalMilliseconds, value, newValue => _postProcessDelay = TimeSpan.FromMilliseconds(newValue));
    }

    public DataSet? DataSet
    {
        get => _dataSet;
        set
        {
            if (!SetProperty(ref _dataSet, value))
                return;
            _availableItemClasses.Clear();
            _itemClasses.Clear();
            if (value == null)
                AvailableWeights = Array.Empty<Weights>();
            else
            {
                AvailableWeights = value.WeightsLibrary.Weights;
                _availableItemClasses.AddRange(value.ItemClasses);
            }
        }
    }

    public Weights? Weights
    {
        get => _weights;
        set => SetProperty(ref _weights, value);
    }

    public IReadOnlyList<ItemClass> ItemClasses { get; }

    protected AbstractProfileEditorVIewModel(IValidator<TProfileData> validator, DataSetsObservableRepository dataSetsObservableRepository, bool canDelete) : base(validator)
    {
        _canDelete = canDelete;
        AvailableDataSets = dataSetsObservableRepository.DataSets;
        _itemClasses.Connect()
            .Bind(out var itemClasses)
            .Subscribe()
            .DisposeWithEx(_constructorDisposables);
        ItemClasses = itemClasses;
        _availableItemClasses.Connect()
            .Except(_itemClasses.Connect())
            .Bind(out var availableItemClasses)
            .Subscribe()
            .DisposeWithEx(_constructorDisposables);
        AvailableItemClasses = availableItemClasses;
    }

    private readonly CompositeDisposable _constructorDisposables = new();
    protected readonly SourceList<ItemClass> _itemClasses = new();
    private readonly SourceList<ItemClass> _availableItemClasses = new();
    private readonly bool _canDelete;
    private DataSet? _dataSet;
    private Weights? _weights;
    private float _mouseSensitivity = 1;
    private float _detectionThreshold = 0.6f;
    private string _description = string.Empty;
    private string _name = string.Empty;
    private IReadOnlyCollection<Weights> _availableWeights = Array.Empty<Weights>();
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddItemClassCommand))]
    private ItemClass? _itemClassToAdd;
    private TimeSpan _postProcessDelay;

    ICommand ProfileEditorViewModel.AddItemClassCommand => AddItemClassCommand;
    [RelayCommand(CanExecute = nameof(CanAddItemClass))]
    private void AddItemClass()
    {
        Guard.IsNotNull(ItemClassToAdd);
        _itemClasses.Add(ItemClassToAdd);
        MoveItemClassUpCommand.NotifyCanExecuteChanged();
        MoveItemClassDownCommand.NotifyCanExecuteChanged();
    }
    private bool CanAddItemClass() => ItemClassToAdd != null;

    ICommand ProfileEditorViewModel.RemoveItemClassCommand => RemoveItemClassCommand;
    [RelayCommand]
    private void RemoveItemClass(ItemClass itemClass)
    {
        Guard.IsTrue(_itemClasses.Remove(itemClass));
        MoveItemClassUpCommand.NotifyCanExecuteChanged();
        MoveItemClassDownCommand.NotifyCanExecuteChanged();
    }

    ICommand ProfileEditorViewModel.MoveItemClassUpCommand => MoveItemClassUpCommand;
    [RelayCommand(CanExecute = nameof(CanMoveItemClassUp))]
    private void MoveItemClassUp(ItemClass itemClass)
    {
        var currentIndex = ItemClasses.IndexOf(itemClass);
        _itemClasses.Move(currentIndex, currentIndex - 1);
        MoveItemClassUpCommand.NotifyCanExecuteChanged();
        MoveItemClassDownCommand.NotifyCanExecuteChanged();
    }
    private bool CanMoveItemClassUp(ItemClass itemClass)
    {
        var currentIndex = ItemClasses.IndexOf(itemClass);
        return currentIndex > 0;
    }

    ICommand ProfileEditorViewModel.MoveItemClassDownCommand => MoveItemClassDownCommand;
    [RelayCommand(CanExecute = nameof(CanMoveItemClassDown))]
    private void MoveItemClassDown(ItemClass itemClass)
    {
        var currentIndex = ItemClasses.IndexOf(itemClass);
        _itemClasses.Move(currentIndex, currentIndex + 1);
        MoveItemClassUpCommand.NotifyCanExecuteChanged();
        MoveItemClassDownCommand.NotifyCanExecuteChanged();
    }
    private bool CanMoveItemClassDown(ItemClass itemClass)
    {
        var currentIndex = ItemClasses.IndexOf(itemClass);
        return currentIndex < ItemClasses.Count - 1;
    }

    ICommand ProfileEditorViewModel.ApplyCommand => ApplyCommand;
    [RelayCommand(CanExecute = nameof(CanApply))]
    private async Task Apply()
    {
        var isValid = await Validate();
        if (isValid)
            Return(ProfileEditorResult.Apply);
    }
    private bool CanApply() => !HasErrors;

    ICommand ProfileEditorViewModel.DeleteCommand => DeleteCommand;

    [RelayCommand]
    private void Delete()
    {
        Return(ProfileEditorResult.Delete);
    }
    private bool CanDelete() => _canDelete;
}