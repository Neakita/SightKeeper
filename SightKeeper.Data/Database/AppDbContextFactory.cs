using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Data.Database;

public interface AppDbContextFactory : IDbContextFactory<AppDbContext>;