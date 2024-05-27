using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.MessageBoxDialog;
using SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.ItemClass;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.DataSet;

internal sealed partial class DataSetEditingViewModel : DialogViewModel<bool>, INotifyDataErrorInfo, IDataSetEditorViewModel, DataSetChanges, IDisposable
{
	public ViewModelValidator<DataSetChanges> Validator { get; }
    IReadOnlyCollection<ItemClassInfo> DataSetInfo.ItemClasses =>
        _itemClasses.Select(itemClass => itemClass.ToItemClassInfo()).ToList();
    public IReadOnlyCollection<EditableItemClass> ItemClasses => _itemClasses;
    public IReadOnlyCollection<Game> Games => _gamesDataAccess.Games;
    public Domain.Model.DataSets.DataSet DataSet { get; private set; }

    public DataSetEditingViewModel(Domain.Model.DataSets.DataSet dataSet, IValidator<DataSetChanges> validator, GamesDataAccess gamesDataAccess, DialogManager dialogManager)
    {
	    Validator = new ViewModelValidator<DataSetChanges>(validator, this, this);
        _gamesDataAccess = gamesDataAccess;
        _dialogManager = dialogManager;
        SetData(dataSet);
        ErrorsChanged += OnErrorsChanged;
    }

    private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
	    ApplyCommand.NotifyCanExecuteChanged();
    }

    [MemberNotNull(nameof(DataSet))]
    private void SetData(Domain.Model.DataSets.DataSet dataSet)
    {
        DataSet = dataSet;
        _itemClasses.Clear();
        foreach (var itemClass in dataSet.ItemClasses)
            _itemClasses.Add(new ExistingItemClass(itemClass));
        Name = dataSet.Name;
        Description = dataSet.Description;
        Resolution = dataSet.Resolution;
        Game = dataSet.Game;
    }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddItemClassCommand))]
    private string _newItemClassName = string.Empty;

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ApplyCommand))] private int? _resolution = 320;
    [ObservableProperty] private Game? _game;

    private readonly ObservableCollection<EditableItemClass> _itemClasses = new();
    private readonly List<DeletedItemClass> _deletedItemClasses = new();
    private readonly GamesDataAccess _gamesDataAccess;
    private readonly DialogManager _dialogManager;

    ICommand IDataSetEditorViewModel.AddItemClassCommand => AddItemClassCommand;
    [RelayCommand(CanExecute = nameof(CanAddItemClass))]
    private async Task AddItemClass()
    {
        var deletedItemClassWithTheSameName = _deletedItemClasses.FirstOrDefault(deletedItemClass => deletedItemClass.ItemClass.Name == NewItemClassName);
        if (deletedItemClassWithTheSameName != null)
        {
            var noButton = new MessageBoxButtonDefinition("No", MaterialIconKind.Close);
            MessageBoxDialogViewModel messageBox = new(
                "Item class adding",
                $"Item class {NewItemClassName} already exists, but it was deleted. Would you like to re-add it?",
                new MessageBoxButtonDefinition("Yes", MaterialIconKind.Check),
                noButton);
            var result = await _dialogManager.ShowDialogAsync(messageBox);
            if (result == noButton)
                return;
            var isRemoved = _deletedItemClasses.Remove(deletedItemClassWithTheSameName);
            Guard.IsTrue(isRemoved);
            _itemClasses.Add(new ExistingItemClass(deletedItemClassWithTheSameName.ItemClass));
        }
        else
        {
            _itemClasses.Add(new NewItemClassViewModel(NewItemClassName, (byte)_itemClasses.Count));
        }
        NewItemClassName = string.Empty;
    }

    private bool CanAddItemClass() =>
        !string.IsNullOrWhiteSpace(NewItemClassName) &&
        ItemClasses.All(existingItemClass => existingItemClass.Name != NewItemClassName) &&
        _itemClasses.Count < 80;

    ICommand IDataSetEditorViewModel.DeleteItemClassCommand => DeleteItemClassCommand;
    [RelayCommand]
    private async Task DeleteItemClass(EditableItemClass itemClass)
    {
        // ISSUE HUGE mess!
        if (itemClass is ExistingItemClass existingItemClass)
        {
            DeletedItemClassAction? action = null;
            if (existingItemClass.ItemClass.Items.Any())
            {
                var cancel = new MessageBoxButtonDefinition("Cancel", MaterialIconKind.Cancel);
                var deleteItems = new MessageBoxButtonDefinition("Delete items", MaterialIconKind.TrashCanOutline);
                var deleteAssets = new MessageBoxButtonDefinition("Delete assets", MaterialIconKind.TrashCanOutline);
                var deleteScreenshots = new MessageBoxButtonDefinition("Delete screenshots", MaterialIconKind.TrashCanOutline);
                MessageBoxDialogViewModel messageBox = new(
                    "Associated items",
                    $"Item class {itemClass.Name} has some items ({existingItemClass.ItemClass.Items.Count}). Are you sure you want to delete it? Choose action",
                    cancel, deleteItems, deleteAssets, deleteScreenshots);
                var result = await _dialogManager.ShowDialogAsync(messageBox);
                if (result == cancel)
                    return;
                if (result == deleteItems)
                    action = DeletedItemClassAction.DeleteItems;
                else if (result == deleteAssets)
                    action = DeletedItemClassAction.DeleteAssets;
                else if (result == deleteScreenshots)
                    action = DeletedItemClassAction.DeleteScreenshots;
            }
            _deletedItemClasses.Add(new DeletedItemClass(existingItemClass.ItemClass, action));
        }
        Guard.IsTrue(_itemClasses.Remove(itemClass));
    }

    ICommand IDataSetEditorViewModel.ApplyCommand => ApplyCommand;
    [RelayCommand(CanExecute = nameof(CanApply))]
    private void Apply()
    {
        Guard.IsFalse(HasErrors);
        Return(true);
    }

    private bool CanApply() => !HasErrors;

    ICommand IDataSetEditorViewModel.CancelCommand => CancelCommand;
    [RelayCommand]
    private void Cancel() => Return(false);

    public void Dispose()
    {
	    ErrorsChanged -= OnErrorsChanged;
    }

    IReadOnlyCollection<ItemClassInfo> DataSetChanges.NewItemClasses => ItemClasses
        .OfType<NewItemClassViewModel>()
        .Select(newItemClass => newItemClass.ToItemClassInfo())
        .ToList();
    IReadOnlyCollection<EditedItemClass> DataSetChanges.EditedItemClasses => ItemClasses
        .OfType<ExistingItemClass>()
        .Where(existingItemClass => existingItemClass.IsEdited)
        .Select(existingItemClass => existingItemClass.ToEditedItemClass())
        .ToList();
    IReadOnlyCollection<DeletedItemClass> DataSetChanges.DeletedItemClasses => _deletedItemClasses;

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

    public override string Header => "Edit Dataset";

    protected override bool DefaultResult => false;
}