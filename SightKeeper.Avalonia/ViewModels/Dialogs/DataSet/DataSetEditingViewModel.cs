using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Dialogs.Abstract;
using SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.ItemClass;
using SightKeeper.Domain.Model;
using SightKeeper.Services.Games;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.DataSet;

public sealed partial class DataSetEditingViewModel : ValidatableDialogViewModel<DataSetChanges, bool>, IDataSetEditorViewModel, DataSetChanges, IDisposable
{
    IReadOnlyCollection<ItemClassInfo> DataSetInfo.ItemClasses =>
        _itemClasses.Select(itemClass => itemClass.ToItemClassInfo()).ToList();
    public IReadOnlyCollection<EditableItemClass> ItemClasses => _itemClasses;
    public Task<IReadOnlyCollection<Game>> Games => _registeredGamesService.GetRegisteredGames();
    public Domain.Model.DataSet DataSet { get; private set; }

    public DataSetEditingViewModel(Domain.Model.DataSet dataSet, IValidator<DataSetChanges> validator, RegisteredGamesService registeredGamesService, AssetsDataAccess assetsDataAccess, ItemClassDataAccess itemClassDataAccess) : base(validator)
    {
        _registeredGamesService = registeredGamesService;
        _assetsDataAccess = assetsDataAccess;
        _itemClassDataAccess = itemClassDataAccess;
        SetData(dataSet);
        _disposable = ErrorsChangedObservable.Subscribe(_ => ApplyCommand.NotifyCanExecuteChanged());
    }

    private readonly IDisposable _disposable;

    [MemberNotNull(nameof(DataSet))]
    private void SetData(Domain.Model.DataSet dataSet)
    {
        DataSet = dataSet;
        _itemClasses.Clear();
        foreach (var itemClass in dataSet.ItemClasses)
            _itemClasses.Add(new ExistingItemClass(itemClass));
        Name = dataSet.Name;
        Description = dataSet.Description;
        Resolution = dataSet.Resolution;
        Game = dataSet.Game;
        _assetsDataAccess.LoadAssetsAsync(dataSet);
        foreach (var asset in dataSet.Assets)
            _assetsDataAccess.LoadItemsAsync(asset);
    }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddItemClassCommand))]
    private string _newItemClassName = string.Empty;

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ApplyCommand))] private int? _resolution = 320;
    [ObservableProperty] private Game? _game;

    private readonly ObservableCollection<EditableItemClass> _itemClasses = new();
    private readonly List<DeletedItemClass> _deletedItemClasses = new();
    private readonly RegisteredGamesService _registeredGamesService;
    private readonly AssetsDataAccess _assetsDataAccess;
    private readonly ItemClassDataAccess _itemClassDataAccess;

    ICommand IDataSetEditorViewModel.AddItemClassCommand => AddItemClassCommand;
    [RelayCommand(CanExecute = nameof(CanAddItemClass))]
    private async Task AddItemClass()
    {
        var deletedItemClassWithTheSameName = _deletedItemClasses.FirstOrDefault(deletedItemClass => deletedItemClass.Tag.Name == NewItemClassName);
        if (deletedItemClassWithTheSameName != null)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams()
            {
                ContentMessage =
                    $"Item class {NewItemClassName} already exists, but it was deleted. Would you like to re-add it?",
                ButtonDefinitions = ButtonEnum.YesNo
            });
            var result = await messageBox.ShowWindowDialogAsync(this.GetOwnerWindow());
            if (result == ButtonResult.No)
                return;
            Guard.IsTrue(_deletedItemClasses.Remove(deletedItemClassWithTheSameName));
            _itemClasses.Add(new ExistingItemClass(deletedItemClassWithTheSameName.Tag));
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
            await _itemClassDataAccess.LoadItemsAsync(existingItemClass.Tag);
            if (existingItemClass.Tag.Items.Any())
            {
                const string deleteItems = "Delete items";
                const string deleteAssets = "Delete assets";
                const string deleteScreenshots = "Delete screenshots";
                var messageBox = MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
                {
                    ContentMessage = $"Item class {itemClass.Name} has some items ({existingItemClass.Tag.Items.Count}). Are you sure you want to delete it? Choose action",
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "Cancel", IsCancel = true },
                        new ButtonDefinition { Name = deleteItems },
                        new ButtonDefinition { Name = deleteAssets },
                        new ButtonDefinition { Name = deleteScreenshots }
                    }
                });
                var result = await messageBox.ShowWindowDialogAsync(this.GetOwnerWindow());
                if (result is null or "Cancel")
                    return;
                action = result switch
                {
                    deleteItems => DeletedItemClassAction.DeleteItems,
                    deleteAssets => DeletedItemClassAction.DeleteAssets,
                    deleteScreenshots => DeletedItemClassAction.DeleteScreenshots,
                    _ => ThrowHelper.ThrowArgumentOutOfRangeException<DeletedItemClassAction>(nameof(result))
                };
            }
            _deletedItemClasses.Add(new DeletedItemClass(existingItemClass.Tag, action));
        }
        Guard.IsTrue(_itemClasses.Remove(itemClass));
    }

    ICommand IDataSetEditorViewModel.ApplyCommand => ApplyCommand;
    [RelayCommand(CanExecute = nameof(CanApply))]
    private async Task Apply()
    {
        var isValid = await Validate();
        if (!isValid)
            return;
        Return(true);
    }

    private bool CanApply() => !HasErrors;

    ICommand IDataSetEditorViewModel.CancelCommand => CancelCommand;
    [RelayCommand]
    private void Cancel() => Return(false);

    public void Dispose()
    {
        _disposable.Dispose();
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
}