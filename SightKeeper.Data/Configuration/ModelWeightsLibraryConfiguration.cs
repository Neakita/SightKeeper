using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ModelWeightsLibraryConfiguration : IEntityTypeConfiguration<ModelWeightsLibrary>
{
    public void Configure(EntityTypeBuilder<ModelWeightsLibrary> builder)
    {
        builder.ToTable("ModelWeightsLibraries");
        builder.HasShadowKey();
        builder.HasMany(library => library.Weights).WithOne(weights => weights.Library).IsRequired();
        builder.HasOne(library => library.DataSet).WithOne(model => model.WeightsLibrary).HasPrincipalKey<DataSet>();
    }
}