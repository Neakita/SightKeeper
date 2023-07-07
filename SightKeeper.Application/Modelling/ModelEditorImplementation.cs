using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Modelling;

public sealed class ModelEditorImplementation : ModelEditor
{
    public ModelEditorImplementation(IValidator<ModelChanges> changesValidator)
    {
        _changesValidator = changesValidator;
    }
    
    public void ApplyChanges(Model model, ModelChanges changes)
    {
        var validationResult = _changesValidator.Validate(changes);
        if (!validationResult.IsValid)
            ThrowHelper.ThrowArgumentException($"Invalid model changes: {validationResult}");
        model.Name = changes.Name;
        model.Description = changes.Description;
        model.Resolution = new Resolution(changes.ResolutionWidth, changes.ResolutionHeight);
        model.Game = changes.Game;
        model.Config = changes.Config;
        ApplyItemClassesChanges(model, changes.ItemClasses);
    }

    private static void ApplyItemClassesChanges(Model model, IReadOnlyCollection<string> itemClasses)
    {
        var deletedItemClasses = GetDeletedItemClasses(model, itemClasses);
        var addedItemClasses = GetAddedItemClasses(model, itemClasses);
        foreach (var deletedItemClass in deletedItemClasses)
            model.DeleteItemClass(deletedItemClass);
        foreach (var addedItemClass in addedItemClasses)
            model.CreateItemClass(addedItemClass);
    }

    private static IReadOnlyCollection<ItemClass> GetDeletedItemClasses(Model model, IReadOnlyCollection<string> itemClasses) =>
        model.ItemClasses.Where(existingItemClass => !itemClasses.Contains(existingItemClass.Name)).ToList();

    private static IReadOnlyCollection<string> GetAddedItemClasses(Model model, IReadOnlyCollection<string> itemClasses) =>
        itemClasses.Where(newItemClass => model.ItemClasses.All(existingItemClass => existingItemClass.Name != newItemClass)).ToList();

    private readonly IValidator<ModelChanges> _changesValidator;
}