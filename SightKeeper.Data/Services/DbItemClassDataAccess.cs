using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbItemClassDataAccess : DbDataAccess<ItemClass>, ItemClassDataAccess
{
    public DbItemClassDataAccess(AppDbContext dbContext) : base(dbContext)
    {
    }
    
    public Task LoadItems(ItemClass itemClass, CancellationToken cancellationToken) =>
        EnsureCollectionLoaded(itemClass, x => x.Items, cancellationToken);
}