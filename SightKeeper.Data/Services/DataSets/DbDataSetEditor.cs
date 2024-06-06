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
        ApplyTagsChanges(dataSetChanges);
        _dbContext.DataSets.Update(dataSet);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dataSetEdited.OnNext(dataSet);
    }

    private void ApplyTagsChanges(DataSetChangesDTO changes)
    {
        DeleteTags(changes);
        ApplyTagsEditions(changes);
        AddNewTags(changes);
    }

    private void DeleteTags(DataSetChangesDTO changes)
    {
        var deletedTags = changes.DeletedTags;
        var tagsWithItemsAndNoActionSpecified = deletedTags
            .Where(deletedTag => IsTagHaveItems(deletedTag.Tag) && deletedTag.Action == null)
            .ToList();
        if (tagsWithItemsAndNoActionSpecified.Any())
            ThrowHelper.ThrowArgumentException($"Item classes must have an action specified when they have items, but no action was specified for: {string.Join(", ", tagsWithItemsAndNoActionSpecified.Select(tag => tag.Tag.Name))}");
        foreach (var deletedTag in deletedTags)
        {
            ExecuteActionIfNecessary(deletedTag);
            var tag = deletedTag.Tag;
            var dataSet = _objectsLookupper.GetDataSet(tag);
            dataSet.DeleteTag(tag);
        }
    }
    private void ExecuteActionIfNecessary(DeletedTag deletedTag)
    {
        if (!IsTagHaveItems(deletedTag.Tag))
            return;
        Guard.IsNotNull(deletedTag.Action);
        var action = deletedTag.Action switch
        {
            DeletedTagAction.DeleteItems => DeleteItems,
            DeletedTagAction.DeleteAssets => DeleteAssets,
            DeletedTagAction.DeleteScreenshots => DeleteScreenshots,
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<Action<Tag>>(nameof(deletedTag.Action), deletedTag.Action, $"Expected one of {nameof(DeletedTagAction.DeleteItems)}, {nameof(DeletedTagAction.DeleteAssets)}, {nameof(DeletedTagAction.DeleteScreenshots)}")
        };
        action(deletedTag.Tag);
    }
    private void DeleteItems(Tag tag)
    {
        foreach (var item in _objectsLookupper.GetItems(tag))
            _objectsLookupper.GetAsset(item).DeleteItem(item);
    }
    private void DeleteAssets(Tag tag)
    {
        foreach (var asset in _objectsLookupper.GetItems(tag).Select(item => _objectsLookupper.GetAsset(item)).Distinct().ToList())
            _objectsLookupper.GetLibrary(asset).DeleteAsset(asset);
    }
    private void DeleteScreenshots(Tag tag)
    {
        foreach (var screenshot in _objectsLookupper.GetItems(tag).Select(item => _objectsLookupper.GetAsset(item).Screenshot).Distinct().ToList())
	        _objectsLookupper.GetLibrary(screenshot).DeleteScreenshot(screenshot);
    }

    private static void ApplyTagsEditions(DataSetChangesDTO changes)
    {
        var editedTags = changes.EditedTags;
        foreach (var editedTag in editedTags)
        {
            var tag = editedTag.Tag;
            tag.Name = editedTag.Name;
            tag.Color = editedTag.Color;
        }
    }

    private static void AddNewTags(DataSetChangesDTO changes)
    {
        var newTags = changes.NewTags;
        foreach (var newTag in newTags)
            changes.DataSet.CreateTag(newTag.Name, newTag.Color);
    }

    private readonly Subject<DetectorDataSet> _dataSetEdited = new();
    private readonly IValidator<DataSetChangesDTO> _changesValidator;
    private readonly AppDbContext _dbContext;
    private readonly ObjectsLookupper _objectsLookupper;

    private bool IsTagHaveItems(Tag tag)
    {
	    return _objectsLookupper.GetItems(tag).Any();
    }
}