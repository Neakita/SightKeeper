using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Data.Configuration;

public sealed class WeightsConfiguration : IEntityTypeConfiguration<Weights>
{
    public void Configure(EntityTypeBuilder<Weights> builder)
    {
        builder.HasKey(weights => weights.Id);
        builder.HasFlakeId(weights => weights.Id);
        builder.Navigation(weights => weights.Library).AutoInclude();
        builder.HasOne(weights => weights.PTData).WithOne().HasPrincipalKey<Weights>().IsRequired();
        builder.HasOne(weights => weights.ONNXData).WithOne().HasPrincipalKey<Weights>().IsRequired();
        builder.HasMany(weights => weights.ItemClasses).WithMany().UsingEntity<WeightsItemClass>(
            "WeightsItemClasses",
            left => left.HasOne(l => l.ItemClass).WithMany().HasForeignKey("ItemClassId").HasPrincipalKey("Id"),
            right => right.HasOne(r => r.Weights).WithMany().HasForeignKey("WeightsId").HasPrincipalKey("Id"),
            join => join.HasKey("WeightsId", "ItemClassId"));
        builder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);
        builder.Navigation(weights => weights.ItemClasses).AutoInclude();
    }

    public sealed class WeightsItemClass : ObservableObject
    {
        public Weights Weights { get; private set; } = null!;
        public ItemClass ItemClass { get; private set; } = null!;
    }
}