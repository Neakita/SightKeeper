using System.Reactive.Subjects;
using FluentValidation;
using SightKeeper.Application.DataSet.Editing;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Services.DataSet;

public sealed class DbDataSetEditor : DataSetEditor
{
    public IObservable<Domain.Model.DataSet> DataSetEdited => _dataSetEdited;
    
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
        dataSet.Resolution = dataSetChanges.Resolution;
        dataSet.Game = dataSetChanges.Game;
        ApplyItemClassesChanges(dataSet, dataSetChanges.ItemClasses);
        _dbContext.DataSets.Update(dataSet);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dataSetEdited.OnNext(dataSet);
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

    private readonly Subject<Domain.Model.DataSet> _dataSetEdited = new();
    private readonly IValidator<DataSetChangesDTO> _changesValidator;
    private readonly AppDbContext _dbContext;
}