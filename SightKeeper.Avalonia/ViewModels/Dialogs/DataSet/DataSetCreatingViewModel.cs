using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Avalonia.ViewModels.Dialogs.Abstract;
using SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.ItemClass;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.DataSet;

public sealed partial class DataSetCreatingViewModel : ValidatableDialogViewModel<NewDataSetInfo, bool>, IDataSetEditorViewModel, NewDataSetInfo
{
    public IReadOnlyCollection<EditableItemClass> ItemClasses => _itemClasses;
    public IReadOnlyCollection<Game> Games => _gamesDataAccess.Games;

    public DataSetCreatingViewModel(IValidator<NewDataSetInfo> validator, GamesDataAccess gamesDataAccess) : base(validator)
    {
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

    partial void OnResolutionChanged(int? oldValue, int? newValue)
    {
        Debug.WriteLine($"Resolution changed from {oldValue} to {newValue}");
    }

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
    private async Task Apply()
    {
        var isValid = await Validate();
        if (!isValid)
            return;
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
}