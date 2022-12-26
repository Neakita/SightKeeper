using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace SightKeeper.Abstractions;

public interface IDbContext : IDisposable
{
	int SaveChanges();
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	void RollBack();
	void RollBack<TEntity>(TEntity entity) where TEntity : class;

	DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
