using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Configuration;

public sealed class WeightsLibraryConfiguration : IEntityTypeConfiguration<WeightsLibrary>
{
    public void Configure(EntityTypeBuilder<WeightsLibrary> builder)
    {
        builder.HasFlakeIdKey();
        builder.ToTable("WeightsLibraries");
        builder.Navigation(library => library.DataSet).AutoInclude();
        builder.Navigation("_weights").AutoInclude();
    }
}