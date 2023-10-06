using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.Data;

public static class Extensions
{
    private const string IdPropertyName = "Id";

    public static EntityTypeBuilder<TEntity> HasShadowKey<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        builder.Property<long>(IdPropertyName);
        builder.HasKey(IdPropertyName);
        return builder;
    }

    public static PropertyEntry<TEntity, long> IdProperty<TEntity>(this EntityEntry<TEntity> entry) where TEntity : class =>
        entry.Property<long>(IdPropertyName);
}