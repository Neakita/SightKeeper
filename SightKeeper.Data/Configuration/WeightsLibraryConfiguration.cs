using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class WeightsLibraryConfiguration : IEntityTypeConfiguration<WeightsLibrary>
{
    public void Configure(EntityTypeBuilder<WeightsLibrary> builder)
    {
        builder.ToTable("ModelWeightsLibraries");
        builder.HasShadowKey();
        builder.HasMany(library => library.Weights).WithOne(weights => weights.Library).IsRequired();
        builder.HasOne(library => library.DataSet).WithOne(model => model.WeightsLibrary).HasPrincipalKey<DataSet>();
    }
}