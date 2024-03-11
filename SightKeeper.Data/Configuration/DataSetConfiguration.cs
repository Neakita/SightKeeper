using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

public sealed class DataSetConfiguration : IEntityTypeConfiguration<DataSet>
{
    public void Configure(EntityTypeBuilder<DataSet> builder)
    {
        builder.HasFlakeIdKey();
        builder.Property(dataSet => dataSet.Resolution);
        builder.HasMany(dataSet => dataSet.ItemClasses).WithOne(itemClass => itemClass.DataSet);
        builder.HasOne(dataSet => dataSet.Screenshots).WithOne(library => library.DataSet).HasPrincipalKey<DataSet>();
        builder.HasOne(dataSet => dataSet.Assets).WithOne(library => library.DataSet).HasPrincipalKey<DataSet>();
        builder.HasOne(dataSet => dataSet.Weights).WithOne(library => library.DataSet).HasPrincipalKey<DataSet>();
        builder.Navigation(dataSet => dataSet.ItemClasses).AutoInclude();
        builder.Navigation(dataSet => dataSet.Game).AutoInclude();
        builder.Navigation(dataSet => dataSet.Screenshots).AutoInclude();
        builder.Navigation(dataSet => dataSet.Assets).AutoInclude();
        builder.Navigation(dataSet => dataSet.Weights).AutoInclude();
    }
}