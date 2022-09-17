using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.DAL.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
	IQueryable<TEntity> Items { get; }

	void Add(TEntity entity);

	Task AddAsync(TEntity entity, CancellationToken cancellationToken);

	void AddRange(IEnumerable<TEntity> entities);

	Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

	void Remove(TEntity entity);

	Task RemoveAsync(TEntity entity, CancellationToken cancellationToken);

	void RemoveRange(IEnumerable<TEntity> entities);

	Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

	void Update(TEntity entity);

	Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

	void UpdateRange(IEnumerable<TEntity> entities);

	Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

	void Clear();

	Task ClearAsync(CancellationToken cancellationToken);
}