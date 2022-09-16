using Microsoft.EntityFrameworkCore;
using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.DAL.Repositories;

/// <summary>
/// Base Entity Framework repository class which implements <see cref="IRepository{TEntity}"/> interface and can be inherited to specified entity if needed
/// </summary>
/// <typeparam name="TEntity">The type of entity that will be associated with the repository inherited from the <see cref="Entity"/> abstract class</typeparam>
public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
	private readonly DbContext _dbContext;
	private readonly DbSet<TEntity> _set;

	public EFRepository(DbContext dbContext)
	{
		_dbContext = dbContext;
		_set = dbContext.Set<TEntity>();
	}

	public virtual IQueryable<TEntity> Items => _set;

	public void Add(TEntity entity)
	{
		_set.Add(entity);
		_dbContext.SaveChanges();
	}

	public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
	{
		await _set.AddAsync(entity, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}

	public void AddRange(IEnumerable<TEntity> entities)
	{
		_set.AddRange(entities);
		_dbContext.SaveChanges();
	}

	public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
	{
		await _set.AddRangeAsync(entities, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}

	public void Remove(TEntity entity)
	{
		_set.Remove(entity);
		_dbContext.SaveChanges();
	}

	public async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken)
	{
		_set.Remove(entity);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}

	public void RemoveRange(IEnumerable<TEntity> entities)
	{
		_set.RemoveRange(entities);
		_dbContext.SaveChanges();
	}

	public async Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
	{
		_set.RemoveRange(entities);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}

	void IRepository<TEntity>.Update(TEntity entity)
	{
		_set.Update(entity);
		_dbContext.SaveChanges();
	}

	public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
	{
		_set.Update(entity);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}

	public void UpdateRange(IEnumerable<TEntity> entities)
	{
		_set.UpdateRange(entities);
		_dbContext.SaveChanges();
	}

	public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
	{
		_set.UpdateRange(entities);
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}
