﻿namespace SightKeeper.DAL;

public interface IDbContext : IDisposable
{
	int SaveChanges();
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	void RollBack();
	void RollBack<TEntity>(TEntity entity) where TEntity : class;
}