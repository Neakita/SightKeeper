using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Configuration;

public sealed class ModelConfigConfiguration : IEntityTypeConfiguration<ModelConfig>
{
    private const string DefaultFilesDirectoryPath = "Defaults/";
    private const string V8NanoFilePath = DefaultFilesDirectoryPath + "v8n";
    private const string V8SmallFilePath = DefaultFilesDirectoryPath + "v8s";
    private const string V8MediumFilePath = DefaultFilesDirectoryPath + "v8m";
    private const string V8LargeFilePath = DefaultFilesDirectoryPath + "v8l";
    private const string V8XLargeFilePath = DefaultFilesDirectoryPath + "v8x";
    
    public void Configure(EntityTypeBuilder<ModelConfig> builder)
    {
        builder.HasShadowKey();
        builder.HasData(GetDefaultModelConfigs());
    }

    private static IEnumerable<ModelConfig> GetDefaultModelConfigs()
    {
        yield return new ModelConfig("V8 Nano", File.ReadAllBytes(V8NanoFilePath), ModelType.Detector);
        yield return new ModelConfig("V8 Small", File.ReadAllBytes(V8SmallFilePath), ModelType.Detector);
        yield return new ModelConfig("V8 Medium", File.ReadAllBytes(V8MediumFilePath), ModelType.Detector);
        yield return new ModelConfig("V8 Large", File.ReadAllBytes(V8LargeFilePath), ModelType.Detector);
        yield return new ModelConfig("V8 XLarge", File.ReadAllBytes(V8XLargeFilePath), ModelType.Detector);
    }
}