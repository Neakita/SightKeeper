using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class DataSetConfiguration : IEntityTypeConfiguration<DataSet>
{
    public void Configure(EntityTypeBuilder<DataSet> builder)
    {
        builder.HasKey(dataSet => dataSet.Id);
        builder.HasFlakeId(dataSet => dataSet.Id);
        builder.HasIndex(dataSet => dataSet.Name).IsUnique();
        builder.HasOne(dataSet => dataSet.Screenshots).WithOne(library => library.DataSet).HasPrincipalKey<DataSet>();
        builder.HasOne(dataSet => dataSet.Weights).WithOne(library => library.DataSet).HasPrincipalKey<DataSet>();
        builder.Navigation(dataSet => dataSet.Screenshots).AutoInclude();
        builder.Navigation(dataSet => dataSet.Weights).AutoInclude();
        builder.Navigation(dataSet => dataSet.Game).AutoInclude();
        builder.Navigation(dataSet => dataSet.Screenshots).AutoInclude();
    }
}