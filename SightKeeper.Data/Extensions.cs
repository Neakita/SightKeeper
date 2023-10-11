using System.Linq.Expressions;
using FlakeId;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.Data;

public static class Extensions
{
    private const string IdPropertyName = "Id";
    
    public static EntityTypeBuilder<TEntity> HasFlakeId<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, Id>> propertyExpression) where TEntity : class
    {
        builder.Property(propertyExpression)
            .HasValueGenerator<FlakeIdGenerator>()
            .HasConversion<long>(id => id, number => new Id(number));
        return builder;
    }
}