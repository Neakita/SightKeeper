using FlakeId;

namespace SightKeeper.Data.Tests;

internal static class EFExtensions
{
	public static Id GetId<TEntity>(this TEntity entity, AppDbContext dbContext) where TEntity : notnull
	{
		return (Id)dbContext.Entry(entity).Property("Id").CurrentValue;
	}
}