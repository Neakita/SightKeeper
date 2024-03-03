using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ItemClassDataAccess
{
	void LoadItems(ItemClass itemClass);
    Task LoadItemsAsync(ItemClass itemClass, CancellationToken cancellationToken = default);
}