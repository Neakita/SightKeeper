using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface ItemClassDataAccess
{
    Task LoadItems(ItemClass itemClass, CancellationToken cancellationToken = default);
}