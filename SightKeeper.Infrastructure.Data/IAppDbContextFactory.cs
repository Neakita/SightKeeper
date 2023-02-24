using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Infrastructure.Data;

public interface IAppDbContextFactory : IDbContextFactory<AppDbContext>
{
}