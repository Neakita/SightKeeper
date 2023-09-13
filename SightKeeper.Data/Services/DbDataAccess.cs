using System.Linq.Expressions;

namespace SightKeeper.Data.Services;

public abstract class DbDataAccess<TEntity> where TEntity : class
{
    protected DbDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected Task EnsureCollectionLoaded<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken)
        where TProperty : class
    {
        lock (_dbContext)
            return _dbContext.Entry(entity).Collection(propertyExpression).LoadAsync(cancellationToken: cancellationToken);
    }

    private readonly AppDbContext _dbContext;
}