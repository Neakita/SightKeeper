using System.Reactive.Subjects;
using FluentValidation;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Services.Model;

public sealed class DbModelEditor : ModelEditor
{
    public IObservable<Domain.Model.Model> ModelEdited => _modelEdited;
    
    public DbModelEditor(IValidator<ModelChanges> changesValidator, AppDbContext dbContext)
    {
        _changesValidator = changesValidator;
        _dbContext = dbContext;
    }

    public async Task ApplyChanges(ModelChangesDTO changes, CancellationToken cancellationToken = default)
    {
        await _changesValidator.ValidateAndThrowAsync(changes, cancellationToken);
        var model = changes.Model;
        model.Name = changes.Name;
        model.Description = changes.Description;
        model.Resolution = new Resolution(changes.ResolutionWidth, changes.ResolutionHeight);
        model.Game = changes.Game;
        ApplyItemClassesChanges(model, changes.ItemClasses);
        _dbContext.Models.Update(model);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _modelEdited.OnNext(model);
    }

    private static void ApplyItemClassesChanges(Domain.Model.Model model, IReadOnlyCollection<string> itemClasses)
    {
        var deletedItemClasses = GetDeletedItemClasses(model, itemClasses);
        var addedItemClasses = GetAddedItemClasses(model, itemClasses);
        foreach (var deletedItemClass in deletedItemClasses)
            model.DeleteItemClass(deletedItemClass);
        foreach (var addedItemClass in addedItemClasses)
            model.CreateItemClass(addedItemClass);
    }

    private static IEnumerable<ItemClass> GetDeletedItemClasses(Domain.Model.Model model, IReadOnlyCollection<string> itemClasses) =>
        model.ItemClasses.Where(existingItemClass => !itemClasses.Contains(existingItemClass.Name)).ToList();

    private static IEnumerable<string> GetAddedItemClasses(Domain.Model.Model model, IReadOnlyCollection<string> itemClasses) =>
        itemClasses.Where(newItemClass => model.ItemClasses.All(existingItemClass => existingItemClass.Name != newItemClass)).ToList();

    private readonly Subject<Domain.Model.Model> _modelEdited = new();
    private readonly IValidator<ModelChanges> _changesValidator;
    private readonly AppDbContext _dbContext;
}