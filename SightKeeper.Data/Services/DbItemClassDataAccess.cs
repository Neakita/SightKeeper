using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbItemClassDataAccess : ItemClassDataAccess
{
    public DbItemClassDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void LoadItems(ItemClass itemClass)
    {
        var entry = _dbContext.Entry(itemClass);
        if (entry.State == EntityState.Detached)
            return;
        entry.Collection(x => x.DetectorItems).Load();
    }
    
    private readonly AppDbContext _dbContext;
}