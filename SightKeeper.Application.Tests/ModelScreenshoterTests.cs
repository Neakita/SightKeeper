using Moq;
using SightKeeper.Application.Annotating;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Tests;

public sealed class ModelScreenshoterTests
{
    [Fact]
    public void ShouldSetModel()
    {
        DetectorModel model = new("Test model");
        Mock<ScreenCapture> screenCaptureMock = new();
        Screenshoter screenshoter = new(screenCaptureMock.Object);
        ModelScreenshoter modelScreenshoter = new(screenCaptureMock.Object, screenshoter);
        modelScreenshoter.Model = model;
        modelScreenshoter.Model.Should().Be(model);
    }
    
    [Fact]
    public void ShouldSetModelToNull()
    {
        DetectorModel model = new("Test model");
        Mock<ScreenCapture> screenCaptureMock = new();
        Screenshoter screenshoter = new(screenCaptureMock.Object);
        ModelScreenshoter modelScreenshoter = new(screenCaptureMock.Object, screenshoter);
        modelScreenshoter.Model = model;
        modelScreenshoter.Model = null;
        modelScreenshoter.Model.Should().BeNull();
    }
    
    [Fact]
    public void ShouldNotSetModelWhenScreenshoterAlreadyHasDifferentLibrary()
    {
        ScreenshotsLibrary library = new();
        DetectorModel model = new("Test model");
        Mock<ScreenCapture> screenCaptureMock = new();
        Screenshoter screenshoter = new(screenCaptureMock.Object);
        ModelScreenshoter modelScreenshoter = new(screenCaptureMock.Object, screenshoter);
        screenshoter.Library = library;
        Assert.Throws<InvalidOperationException>(() => modelScreenshoter.Model = model);
    }
}