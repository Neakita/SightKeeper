using SightKeeper.Domain.Model.DataSet;

namespace SightKeeper.Application.DataSet.Editing;

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