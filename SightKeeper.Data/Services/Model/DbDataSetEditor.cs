using System.Reactive.Subjects;
using FluentValidation;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Services.Model;

public sealed class DbDataSetEditor : DataSetEditor
{
    public IObservable<Domain.Model.DataSet> ModelEdited => _modelEdited;
    
    public DbDataSetEditor(IValidator<DataSetDataChanges> changesValidator, AppDbContext dbContext)
    {
        _changesValidator = changesValidator;
        _dbContext = dbContext;
    }

    public async Task ApplyChanges(DataSetDataSetDataChangesDTO dataSetDataChanges, CancellationToken cancellationToken = default)
    {
        await _changesValidator.ValidateAndThrowAsync(dataSetDataChanges, cancellationToken);
        var model = dataSetDataChanges.DataSet;
        model.Name = dataSetDataChanges.Name;
        model.Description = dataSetDataChanges.Description;
        model.Resolution = new Resolution(dataSetDataChanges.ResolutionWidth, dataSetDataChanges.ResolutionHeight);
        model.Game = dataSetDataChanges.Game;
        ApplyItemClassesChanges(model, dataSetDataChanges.ItemClasses);
        _dbContext.Models.Update(model);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _modelEdited.OnNext(model);
    }

    private static void ApplyItemClassesChanges(Domain.Model.DataSet dataSet, IReadOnlyCollection<string> itemClasses)
    {
        var deletedItemClasses = GetDeletedItemClasses(dataSet, itemClasses);
        var addedItemClasses = GetAddedItemClasses(dataSet, itemClasses);
        foreach (var deletedItemClass in deletedItemClasses)
            dataSet.DeleteItemClass(deletedItemClass);
        foreach (var addedItemClass in addedItemClasses)
            dataSet.CreateItemClass(addedItemClass);
    }

    private static IEnumerable<ItemClass> GetDeletedItemClasses(Domain.Model.DataSet dataSet, IReadOnlyCollection<string> itemClasses) =>
        dataSet.ItemClasses.Where(existingItemClass => !itemClasses.Contains(existingItemClass.Name)).ToList();

    private static IEnumerable<string> GetAddedItemClasses(Domain.Model.DataSet dataSet, IReadOnlyCollection<string> itemClasses) =>
        itemClasses.Where(newItemClass => dataSet.ItemClasses.All(existingItemClass => existingItemClass.Name != newItemClass)).ToList();

    private readonly Subject<Domain.Model.DataSet> _modelEdited = new();
    private readonly IValidator<DataSetDataChanges> _changesValidator;
    private readonly AppDbContext _dbContext;
}