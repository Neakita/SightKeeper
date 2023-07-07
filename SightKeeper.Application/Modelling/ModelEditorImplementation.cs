using CommunityToolkit.Diagnostics;
using FluentValidation;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Model;

namespace SightKeeper.Application.Modelling;

public sealed class ModelEditorImplementation : ModelEditor
{
    public ModelEditorImplementation(IValidator<IModel> modelValidator)
    {
        _modelValidator = modelValidator;
    }
    
    public void ApplyChanges(Model model, ModelChanges changes)
    {
        var validationResult = _modelValidator.Validate(changes);
        if (!validationResult.IsValid)
            ThrowHelper.ThrowArgumentException($"Invalid model changes: {validationResult}");
        model.Name = changes.Name;
        model.Description = changes.Description;
        model.Resolution = changes.Resolution;
        model.Game = changes.Game;
        model.Config = changes.Config;
        ApplyItemClassesChanges(model, changes.ItemClasses);
    }

    private static void ApplyItemClassesChanges(Model model, IReadOnlyCollection<ItemClass> itemClasses)
    {
        var deletedItemClasses = model.ItemClasses.Except(itemClasses).ToList();
        var addedItemClasses = itemClasses.Except(model.ItemClasses).ToList();
        foreach (var deletedItemClass in deletedItemClasses)
            model.DeleteItemClass(deletedItemClass);
        foreach (var addedItemClass in addedItemClasses)
            model.AddItemClass(addedItemClass);
    }
    
    private readonly IValidator<IModel> _modelValidator;
}