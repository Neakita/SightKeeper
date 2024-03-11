using FlakeId;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.Data;

public static class Extensions
{
    public static KeyBuilder HasFlakeIdKey<TEntity>(this EntityTypeBuilder<TEntity> builder, string name = "Id") where TEntity : class
    {
	    builder.HasFlakeId(name);
	    return builder.HasKey(name);
    }

    private static EntityTypeBuilder<TEntity> HasFlakeId<TEntity>(this EntityTypeBuilder<TEntity> builder, string name) where TEntity : class
    {
	    builder.Property<Id>(name)
		    .HasValueGenerator<FlakeIdGenerator>()
		    .HasConversion<long>(id => id, number => new Id(number));
	    return builder;
    }
}