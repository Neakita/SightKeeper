using SightKeeper.Application.Config;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Tests;

public sealed class ModelConfigDataValidatorTests
{
    private static readonly string YoloV3ValidConfig = File.ReadAllText("Samples/YoloV3ValidConfig");
    
    [Fact]
    public void ShouldBeValid()
    {
        var validationResult = Validator.Validate(new ConfigDataImplementation(YoloV3ValidConfig));
        validationResult.Errors.Should().BeEmpty();
    }

    [Fact]
    public void EmptyShouldNotBeValid()
    {
        var validationResult = Validator.Validate(new ConfigDataImplementation(string.Empty));
        validationResult.IsValid.Should().BeFalse();
    }
    
    private sealed class ConfigDataImplementation : ConfigData
    {
        public string Name => "Test config";
        public string Content { get; }
        public ModelType ModelType => ModelType.Detector;

        public ConfigDataImplementation(string content)
        {
            Content = content;
        }
    }

    private static readonly ConfigDataValidator Validator = new();
}