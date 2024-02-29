using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.DataSets.Editing;

public sealed class DeletedItemClass
{
    public ItemClass ItemClass { get; }
    public DeletedItemClassAction? Action { get; }

    public DeletedItemClass(ItemClass itemClass, DeletedItemClassAction? action = null)
    {
        ItemClass = itemClass;
        Action = action;
    }
}