using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface ItemClassDataAccess
{
    void LoadItems(ItemClass itemClass);
}