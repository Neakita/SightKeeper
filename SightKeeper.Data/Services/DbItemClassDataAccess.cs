using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbItemClassDataAccess : DbDataAccess<ItemClass>, ItemClassDataAccess
{
    public DbItemClassDataAccess(AppDbContext dbContext) : base(dbContext)
    {
    }

    public void LoadItems(ItemClass itemClass)
    {
	    EnsureCollectionLoaded(itemClass, x => x.Items);
    }

    public Task LoadItemsAsync(ItemClass itemClass, CancellationToken cancellationToken) =>
        EnsureCollectionLoadedAsync(itemClass, x => x.Items, cancellationToken);
}