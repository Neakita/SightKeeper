using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Application.Model;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Services.Games;

namespace SightKeeper.Avalonia.ViewModels;

public partial class ModelEditorViewModel : ValidatableViewModel<ModelData>, DialogViewModel, ModelData
{
    public IReadOnlyCollection<string> ItemClasses => _itemClasses;
    public Task<IReadOnlyCollection<Game>> Games => _registeredGamesService.GetRegisteredGames();
    public IReadOnlyCollection<ModelConfig> Configs => new List<ModelConfig>();

    public ModelEditorViewModel(IValidator<ModelData> validator, RegisteredGamesService registeredGamesService) : base(validator)
    {
        _registeredGamesService = registeredGamesService;
    }

    public void SetData(ModelData data)
    {
        _itemClasses.Clear();
        foreach (var itemClass in data.ItemClasses)
            _itemClasses.Add(itemClass);
        Name = data.Name;
        Description = data.Description;
        ResolutionWidth = data.ResolutionWidth;
        ResolutionHeight = data.ResolutionHeight;
        Game = data.Game;
        Config = data.Config;
    }

    public void SetData(Model model)
    {
        _itemClasses.Clear();
        foreach (var itemClass in model.ItemClasses)
            _itemClasses.Add(itemClass.Name);
        Name = model.Name;
        Description = model.Description;
        ResolutionWidth = model.Resolution.Width;
        ResolutionHeight = model.Resolution.Height;
        Game = model.Game;
        Config = model.Config;
    }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddItemClassCommand))] private string _newItemClassName = string.Empty;
    [ObservableProperty] private string? _selectedItemClass;

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ApplyCommand))] private int? _resolutionWidth = 320;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ApplyCommand))] private int? _resolutionHeight = 320;
    [ObservableProperty] private Game? _game;
    [ObservableProperty] private ModelConfig? _config;

    private readonly ObservableCollection<string> _itemClasses = new();
    private readonly RegisteredGamesService _registeredGamesService;
    
    partial void OnResolutionWidthChanged(int? oldValue, int? newValue)
    {
        Debug.WriteLine($"ResolutionWidth changed from {oldValue} to {newValue}");
    }

    [RelayCommand(CanExecute = nameof(CanAddItemClass))]
    private void AddItemClass()
    {
        _itemClasses.Add(NewItemClassName);
        NewItemClassName = string.Empty;
    }

    private bool CanAddItemClass() =>
        !string.IsNullOrWhiteSpace(NewItemClassName) && !ItemClasses.Contains(NewItemClassName);

    [RelayCommand(CanExecute = nameof(CanDeleteItemClass))]
    private void DeleteItemClass()
    {
        Guard.IsNotNull(SelectedItemClass);
        if (!_itemClasses.Remove(SelectedItemClass))
            ThrowHelper.ThrowArgumentException(nameof(SelectedItemClass), $"{SelectedItemClass} not removed from {ItemClasses}");
    }

    private bool CanDeleteItemClass() => SelectedItemClass != null;

    [RelayCommand(CanExecute = nameof(CanApply))]
    private void Apply()
    {
        DialogResult = true;
        _closeRequested.OnNext(Unit.Default);
    }

    private bool CanApply() => ValidationResult.IsValid;

    [RelayCommand]
    private void Cancel()
    {
        DialogResult = false;
        _closeRequested.OnNext(Unit.Default);
    }

    #region Dialog

    public bool? DialogResult { get; private set; }
    public IObservable<Unit> CloseRequested => _closeRequested.AsObservable();
    private readonly Subject<Unit> _closeRequested = new();

    #endregion
}