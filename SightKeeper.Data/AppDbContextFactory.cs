using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Data;

public interface AppDbContextFactory : IDbContextFactory<AppDbContext>
{
}