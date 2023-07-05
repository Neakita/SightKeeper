using FluentAssertions;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Tests;

public sealed class ScreenshotsLibraryTests
{
    [Fact]
    public void ShouldCreateScreenshot()
    {
        ScreenshotsLibrary library = new();
        var screenshot = library.CreateScreenshot(new Image(Array.Empty<byte>()));
        library.Screenshots.Should().Contain(screenshot);
    }

    [Fact]
    public void ShouldNotCreateScreenshotFromAlreadyUsedImage()
    {
        ScreenshotsLibrary library = new();
        Image image = new(Array.Empty<byte>());
        library.CreateScreenshot(image);
        Assert.Throws<ArgumentException>(() => library.CreateScreenshot(image));
    }

    [Fact]
    public void ShouldNotAddCreatedScreenshot()
    {
        ScreenshotsLibrary library = new();
        var screenshot = library.CreateScreenshot(new Image(Array.Empty<byte>()));
        Assert.Throws<ArgumentException>(() => library.AddScreenshot(screenshot));
    }

    [Fact]
    public void ShouldTransferScreenshotFromDifferentLibrary()
    {
        ScreenshotsLibrary library1 = new();
        ScreenshotsLibrary library2 = new();
        var screenshot = library1.CreateScreenshot(new Image(Array.Empty<byte>()));
        library1.DeleteScreenshot(screenshot);
        library2.AddScreenshot(screenshot);
        library2.Screenshots.Should().Contain(screenshot);
    }
    
    [Fact]
    public void ShouldNotAddScreenshotFromDifferentLibrary()
    {
        ScreenshotsLibrary library1 = new();
        ScreenshotsLibrary library2 = new();
        var screenshot = library1.CreateScreenshot(new Image(Array.Empty<byte>()));
        Assert.Throws<ArgumentException>(() => library2.AddScreenshot(screenshot));
    }
}