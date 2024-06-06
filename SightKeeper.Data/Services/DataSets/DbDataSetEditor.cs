using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services.DataSets;

public sealed class DbDataSetEditor : DataSetEditor
{
    public IObservable<DetectorDataSet> DataSetEdited => _dataSetEdited;
    
    public DbDataSetEditor(IValidator<DataSetChanges> changesValidator, AppDbContext dbContext, ObjectsLookupper objectsLookupper)
    {
        _changesValidator = changesValidator;
        _dbContext = dbContext;
        _objectsLookupper = objectsLookupper;
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

    private void ApplyItemClassesChanges(DataSetChangesDTO changes)
    {
        DeleteItemClasses(changes);
        ApplyItemClassesEditions(changes);
        AddNewItemClasses(changes);
    }

    private void DeleteItemClasses(DataSetChangesDTO changes)
    {
        var deletedItemClasses = changes.DeletedItemClasses;
        var itemClassesWithItemsAndNoActionSpecified = deletedItemClasses
            .Where(deletedItemClass => IsItemClassHaveItems(deletedItemClass.ItemClass) && deletedItemClass.Action == null)
            .ToList();
        if (itemClassesWithItemsAndNoActionSpecified.Any())
            ThrowHelper.ThrowArgumentException($"Item classes must have an action specified when they have items, but no action was specified for: {string.Join(", ", itemClassesWithItemsAndNoActionSpecified.Select(itemClass => itemClass.ItemClass.Name))}");
        foreach (var deletedItemClass in deletedItemClasses)
        {
            ExecuteActionIfNecessary(deletedItemClass);
            var itemClass = deletedItemClass.ItemClass;
            var dataSet = _objectsLookupper.GetDataSet(itemClass);
            dataSet.DeleteItemClass(itemClass);
        }
    }
    private void ExecuteActionIfNecessary(DeletedItemClass deletedItemClass)
    {
        if (!IsItemClassHaveItems(deletedItemClass.ItemClass))
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
    private void DeleteItems(ItemClass itemClass)
    {
        foreach (var item in _objectsLookupper.GetItems(itemClass))
            _objectsLookupper.GetAsset(item).DeleteItem(item);
    }
    private void DeleteAssets(ItemClass itemClass)
    {
        foreach (var asset in _objectsLookupper.GetItems(itemClass).Select(item => _objectsLookupper.GetAsset(item)).Distinct().ToList())
            _objectsLookupper.GetLibrary(asset).DeleteAsset(asset);
    }
    private void DeleteScreenshots(ItemClass itemClass)
    {
        foreach (var screenshot in _objectsLookupper.GetItems(itemClass).Select(item => _objectsLookupper.GetAsset(item).Screenshot).Distinct().ToList())
	        _objectsLookupper.GetLibrary(screenshot).DeleteScreenshot(screenshot);
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

    private readonly Subject<DetectorDataSet> _dataSetEdited = new();
    private readonly IValidator<DataSetChangesDTO> _changesValidator;
    private readonly AppDbContext _dbContext;
    private readonly ObjectsLookupper _objectsLookupper;

    private bool IsItemClassHaveItems(ItemClass itemClass)
    {
	    return _objectsLookupper.GetItems(itemClass).Any();
    }
}