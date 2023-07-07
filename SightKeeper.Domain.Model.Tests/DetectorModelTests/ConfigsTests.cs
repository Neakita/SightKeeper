using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorModelTests;

public sealed class ConfigsTests
{
    [Fact]
    public void ShouldNotSetConfigForDifferentModelType()
    {
        DetectorModel model = new("Model");
        ModelConfig config = new("Config", string.Empty, ModelType.Classifier);
        Assert.Throws<ArgumentException>(() => model.Config = config);
    }
}