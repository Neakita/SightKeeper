using System.Linq.Expressions;

namespace SightKeeper.Data.Services;

public abstract class DbDataAccess<TEntity> where TEntity : class
{
    protected DbDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected void EnsureCollectionLoaded<TProperty>(
	    TEntity entity,
	    Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression)
	    where TProperty : class
    {
	    _dbContext.Entry(entity).Collection(propertyExpression).Load();
    }

    protected Task EnsureCollectionLoadedAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken)
        where TProperty : class
    {
        return _dbContext.Entry(entity).Collection(propertyExpression).LoadAsync(cancellationToken);
    }

    private readonly AppDbContext _dbContext;
}