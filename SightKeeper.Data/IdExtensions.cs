using CommunityToolkit.Diagnostics;
using FlakeId;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.Data;

public static class IdExtensions
{
	private const string IdPropertyName = "Id";

	public static Id GetId<TEntity>(this TEntity entity, AppDbContext dbContext) where TEntity : notnull
	{
		var currentValue = dbContext.Entry(entity).Property(IdPropertyName).CurrentValue;
		Guard.IsNotNull(currentValue);
		return (Id)currentValue;
	}

	public static void SetId<TEntity>(this EntityEntry<TEntity> entry, Id id) where TEntity : class
	{
		entry.Property<Id>(IdPropertyName).CurrentValue = id;
	}
	
    internal static KeyBuilder HasFlakeIdKey<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
	    builder.HasFlakeId(IdPropertyName);
	    return builder.HasKey(IdPropertyName);
    }

    private static EntityTypeBuilder<TEntity> HasFlakeId<TEntity>(this EntityTypeBuilder<TEntity> builder, string name) where TEntity : class
    {
	    builder.Property<Id>(name)
		    .HasValueGenerator<FlakeIdGenerator>()
		    .HasConversion<long>(id => id, number => new Id(number));
	    return builder;
    }
}