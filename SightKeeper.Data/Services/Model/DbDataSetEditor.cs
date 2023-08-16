using System.Reactive.Subjects;
using FluentValidation;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Services.Model;

public sealed class DbDataSetEditor : DataSetEditor
{
    public IObservable<Domain.Model.DataSet> ModelEdited => _modelEdited;
    
    public DbDataSetEditor(IValidator<ModelChanges> changesValidator, AppDbContext dbContext)
    {
        _changesValidator = changesValidator;
        _dbContext = dbContext;
    }

    public async Task ApplyChanges(DataSetChangesDTO changes, CancellationToken cancellationToken = default)
    {
        await _changesValidator.ValidateAndThrowAsync(changes, cancellationToken);
        var model = changes.DataSet;
        model.Name = changes.Name;
        model.Description = changes.Description;
        model.Resolution = new Resolution(changes.ResolutionWidth, changes.ResolutionHeight);
        model.Game = changes.Game;
        ApplyItemClassesChanges(model, changes.ItemClasses);
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
    private readonly IValidator<ModelChanges> _changesValidator;
    private readonly AppDbContext _dbContext;
}