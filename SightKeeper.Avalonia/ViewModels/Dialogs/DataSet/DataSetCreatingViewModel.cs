using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.ItemClass;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.DataSet;

internal sealed partial class DataSetCreatingViewModel : DialogViewModel<bool>, IDataSetEditorViewModel, NewDataSetInfo, INotifyDataErrorInfo, IDisposable
{
	public ViewModelValidator<NewDataSetInfo> Validator { get; }
    public IReadOnlyCollection<EditableItemClass> ItemClasses => _itemClasses;
    public IReadOnlyCollection<Game> Games => _gamesDataAccess.Games;

    public DataSetCreatingViewModel(IValidator<NewDataSetInfo> validator, GamesDataAccess gamesDataAccess)
    {
	    Validator = new ViewModelValidator<NewDataSetInfo>(validator, this, this);
        _gamesDataAccess = gamesDataAccess;
        ErrorsChanged += OnErrorsChanged;
    }

    private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ApplyCommand.NotifyCanExecuteChanged();
    }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddItemClassCommand))]
    private string _newItemClassName = string.Empty;

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ApplyCommand))] private int? _resolution = 320;
    [ObservableProperty] private Game? _game;

    private readonly ObservableCollection<EditableItemClass> _itemClasses = new();
    private readonly GamesDataAccess _gamesDataAccess;

    ICommand IDataSetEditorViewModel.AddItemClassCommand => AddItemClassCommand;
    ICommand IDataSetEditorViewModel.DeleteItemClassCommand => DeleteItemClassCommand;
    ICommand IDataSetEditorViewModel.ApplyCommand => ApplyCommand;
    ICommand IDataSetEditorViewModel.CancelCommand => CancelCommand;

    [RelayCommand(CanExecute = nameof(CanAddItemClass))]
    private void AddItemClass()
    {
        _itemClasses.Add(new NewItemClassViewModel(NewItemClassName, (byte)ItemClasses.Count));
        NewItemClassName = string.Empty;
    }

    private bool CanAddItemClass() =>
        !string.IsNullOrWhiteSpace(NewItemClassName) && ItemClasses.All(existingItemClass => existingItemClass.Name != NewItemClassName);

    [RelayCommand]
    private void DeleteItemClass(EditableItemClass editableItemClass)
    {
        Guard.IsTrue(_itemClasses.Remove(editableItemClass));
    }

    [RelayCommand(CanExecute = nameof(CanApply))]
    private void Apply()
    {
	    Guard.IsFalse(HasErrors);
        Return(true);
    }

    private bool CanApply() => !HasErrors;

    [RelayCommand]
    private void Cancel()
    {
        Return(false);
    }

    IReadOnlyCollection<ItemClassInfo> DataSetInfo.ItemClasses =>
        ItemClasses.Select(itemClass => itemClass.ToItemClassInfo()).ToList();

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

    public void Dispose()
    {
	    Validator.Dispose();
	    ErrorsChanged -= OnErrorsChanged;
    }

    public override string Header => "Create Dataset";

    protected override bool DefaultResult => false;
}