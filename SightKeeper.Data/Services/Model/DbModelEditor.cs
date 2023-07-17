using FluentValidation;
using SightKeeper.Application.Model;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Services.Model;

public sealed class DbModelEditor : ModelEditor
{
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
        model.Config = changes.Config;
        ApplyItemClassesChanges(model, changes.ItemClasses);
        _dbContext.Models.Update(model);
        await _dbContext.SaveChangesAsync(cancellationToken);
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

    private static IReadOnlyCollection<ItemClass> GetDeletedItemClasses(Domain.Model.Model model, IReadOnlyCollection<string> itemClasses) =>
        model.ItemClasses.Where(existingItemClass => !itemClasses.Contains(existingItemClass.Name)).ToList();

    private static IReadOnlyCollection<string> GetAddedItemClasses(Domain.Model.Model model, IReadOnlyCollection<string> itemClasses) =>
        itemClasses.Where(newItemClass => model.ItemClasses.All(existingItemClass => existingItemClass.Name != newItemClass)).ToList();

    private readonly IValidator<ModelChanges> _changesValidator;
    private readonly AppDbContext _dbContext;
}