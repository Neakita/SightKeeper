using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data;

public static class Extensions
{
    private const string IdPropertyName = "Id";

    public static EntityTypeBuilder<TEntity> HasShadowKey<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        builder.Property<int>(IdPropertyName);
        builder.HasKey(IdPropertyName);
        return builder;
    }

    public static PropertyEntry<TEntity, int> IdProperty<TEntity>(this EntityEntry<TEntity> entry) where TEntity : class =>
        entry.Property<int>(IdPropertyName);

    public static IQueryable<Model> WhereGame(this IQueryable<Model> query, Game game) =>
        query.Where(model => model.Game != null && model.Game.Title == game.Title && model.Game.ProcessName == game.ProcessName);
}