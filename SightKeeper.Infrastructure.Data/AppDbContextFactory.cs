using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Infrastructure.Data;

public interface AppDbContextFactory : IDbContextFactory<AppDbContext>
{
}