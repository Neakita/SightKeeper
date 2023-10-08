using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class WeightsLibraryConfiguration : IEntityTypeConfiguration<WeightsLibrary>
{
    public void Configure(EntityTypeBuilder<WeightsLibrary> builder)
    {
        builder.HasKey(library => library.Id);
        builder.HasFlakeId(library => library.Id);
        builder.ToTable("WeightsLibraries");
        builder.Navigation(library => library.DataSet).AutoInclude();
        builder.Navigation(library => library.Weights).AutoInclude();
    }
}