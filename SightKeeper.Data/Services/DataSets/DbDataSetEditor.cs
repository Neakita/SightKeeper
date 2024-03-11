using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Services.DataSets;

public sealed class DbDataSetEditor : DataSetEditor
{
    public IObservable<DataSet> DataSetEdited => _dataSetEdited;
    
    public DbDataSetEditor(IValidator<DataSetChanges> changesValidator, AppDbContext dbContext)
    {
        _changesValidator = changesValidator;
        _dbContext = dbContext;
    }

    public async Task ApplyChanges(DataSetChangesDTO dataSetChanges, CancellationToken cancellationToken = default)
    {
        await _changesValidator.ValidateAndThrowAsync(dataSetChanges, cancellationToken);
        var dataSet = dataSetChanges.DataSet;
        dataSet.Name = dataSetChanges.Name;
        dataSet.Description = dataSetChanges.Description;
        dataSet.Game = dataSetChanges.Game;
        ApplyItemClassesChanges(dataSetChanges);
        _dbContext.DataSets.Update(dataSet);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dataSetEdited.OnNext(dataSet);
    }

    private static void ApplyItemClassesChanges(DataSetChangesDTO changes)
    {
        DeleteItemClasses(changes);
        ApplyItemClassesEditions(changes);
        AddNewItemClasses(changes);
    }

    private static void DeleteItemClasses(DataSetChangesDTO changes)
    {
        var deletedItemClasses = changes.DeletedItemClasses;
        var itemClassesWithItemsAndNoActionSpecified = deletedItemClasses
            .Where(deletedItemClass => deletedItemClass.ItemClass.Items.Any() && deletedItemClass.Action == null)
            .ToList();
        if (itemClassesWithItemsAndNoActionSpecified.Any())
            ThrowHelper.ThrowArgumentException($"Item classes must have an action specified when they have items, but no action was specified for: {string.Join(", ", itemClassesWithItemsAndNoActionSpecified.Select(itemClass => itemClass.ItemClass.Name))}");
        foreach (var deletedItemClass in deletedItemClasses)
        {
            ExecuteActionIfNecessary(deletedItemClass);
            var itemClass = deletedItemClass.ItemClass;
            itemClass.DataSet.DeleteItemClass(itemClass);
        }
    }
    private static void ExecuteActionIfNecessary(DeletedItemClass deletedItemClass)
    {
        if (!deletedItemClass.ItemClass.Items.Any())
            return;
        Guard.IsNotNull(deletedItemClass.Action);
        var action = deletedItemClass.Action switch
        {
            DeletedItemClassAction.DeleteItems => DeleteItems,
            DeletedItemClassAction.DeleteAssets => DeleteAssets,
            DeletedItemClassAction.DeleteScreenshots => DeleteScreenshots,
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<Action<ItemClass>>(nameof(deletedItemClass.Action), deletedItemClass.Action, $"Expected one of {nameof(DeletedItemClassAction.DeleteItems)}, {nameof(DeletedItemClassAction.DeleteAssets)}, {nameof(DeletedItemClassAction.DeleteScreenshots)}")
        };
        action(deletedItemClass.ItemClass);
    }
    private static void DeleteItems(ItemClass itemClass)
    {
        foreach (var item in itemClass.Items.ToList())
            item.Asset.DeleteItem(item);
    }
    private static void DeleteAssets(ItemClass itemClass)
    {
        foreach (var asset in itemClass.Items.Select(item => item.Asset).Distinct().ToList())
            asset.Library.DeleteAsset(asset);
    }
    private static void DeleteScreenshots(ItemClass itemClass)
    {
        foreach (var screenshot in itemClass.Items.Select(item => item.Asset.Screenshot).Distinct().ToList())
            screenshot.Library.DeleteScreenshot(screenshot);
    }

    private static void ApplyItemClassesEditions(DataSetChangesDTO changes)
    {
        var editedItemClasses = changes.EditedItemClasses;
        foreach (var editedItemClass in editedItemClasses)
        {
            var itemClass = editedItemClass.ItemClass;
            itemClass.Name = editedItemClass.Name;
            itemClass.Color = editedItemClass.Color;
        }
    }

    private static void AddNewItemClasses(DataSetChangesDTO changes)
    {
        var newItemClasses = changes.NewItemClasses;
        foreach (var newItemClass in newItemClasses)
            changes.DataSet.CreateItemClass(newItemClass.Name, newItemClass.Color);
    }

    private readonly Subject<DataSet> _dataSetEdited = new();
    private readonly IValidator<DataSetChangesDTO> _changesValidator;
    private readonly AppDbContext _dbContext;
}