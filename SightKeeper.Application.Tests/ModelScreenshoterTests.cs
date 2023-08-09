using NSubstitute;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Tests;

public sealed class ModelScreenshoterTests
{
    [Fact]
    public void ShouldSetModel()
    {
        DetectorModel model = new("Test model");
        var screenCapture = Substitute.For<ScreenCapture>();
        Screenshoter screenshoter = new(screenCapture, Substitute.For<SelfActivityService>());
        ModelScreenshoter modelScreenshoter = new(screenCapture, screenshoter, Substitute.For<GamesService>());
        modelScreenshoter.Model = model;
        modelScreenshoter.Model.Should().Be(model);
    }
    
    [Fact]
    public void ShouldSetModelToNull()
    {
        DetectorModel model = new("Test model");
        var screenCapture = Substitute.For<ScreenCapture>();
        var selfActivity = Substitute.For<SelfActivityService>();
        var gamesService = Substitute.For<GamesService>();
        Screenshoter screenshoter = new(screenCapture, selfActivity);
        ModelScreenshoter modelScreenshoter = new(screenCapture, screenshoter, gamesService);
        modelScreenshoter.Model = model;
        modelScreenshoter.Model = null;
        modelScreenshoter.Model.Should().BeNull();
    }
}