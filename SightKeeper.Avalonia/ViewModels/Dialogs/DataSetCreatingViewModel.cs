using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Application.DataSet.Creating;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Services.Games;

namespace SightKeeper.Avalonia.ViewModels.Dialogs;

public partial class DataSetCreatingViewModel : ValidatableViewModel<NewDataSetInfo>, IDataSetEditorViewModel, DialogViewModel, NewDataSetInfo
{
    public IReadOnlyCollection<string> ItemClasses => _itemClasses;
    public Task<IReadOnlyCollection<Game>> Games => _registeredGamesService.GetRegisteredGames();

    public DataSetCreatingViewModel(IValidator<NewDataSetInfo> validator, RegisteredGamesService registeredGamesService) : base(validator)
    {
        _registeredGamesService = registeredGamesService;
        ErrorsChanged += OnErrorsChanged;
    }

    private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ApplyCommand.NotifyCanExecuteChanged();
    }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddItemClassCommand))] private string _newItemClassName = string.Empty;
    [ObservableProperty] private string? _selectedItemClass;

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ApplyCommand))] private int? _resolution = 320;
    [ObservableProperty] private Game? _game;

    private readonly ObservableCollection<string> _itemClasses = new();
    private readonly RegisteredGamesService _registeredGamesService;
    private readonly IReadOnlyCollection<string> _deletionBlackListItemClasses = Array.Empty<string>();
    
    ICommand IDataSetEditorViewModel.AddItemClassCommand => AddItemClassCommand;
    ICommand IDataSetEditorViewModel.DeleteItemClassCommand => DeleteItemClassCommand;
    ICommand IDataSetEditorViewModel.ApplyCommand => ApplyCommand;
    ICommand IDataSetEditorViewModel.CancelCommand => CancelCommand;

    partial void OnResolutionChanged(int? oldValue, int? newValue)
    {
        Debug.WriteLine($"Resolution changed from {oldValue} to {newValue}");
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

    private bool CanDeleteItemClass() => SelectedItemClass != null && !_deletionBlackListItemClasses.Contains(SelectedItemClass);

    [RelayCommand(CanExecute = nameof(CanApply))]
    private async Task Apply()
    {
        var isValid = await Validate();
        if (!isValid)
            return;
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