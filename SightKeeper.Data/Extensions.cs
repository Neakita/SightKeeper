using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.Data;

public static class Extensions
{
    public static EntityTypeBuilder<TEntity> HasShadowKey<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        builder.Property<int>("Id");
        builder.HasKey("Id");
        return builder;
    }
}