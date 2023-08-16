using FluentAssertions;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Tests;

public sealed class ScreenshotsLibraryTests
{
    [Fact]
    public void ShouldCreateScreenshot()
    {
        ScreenshotsLibrary library = new();
        var screenshot = library.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        library.Screenshots.Should().Contain(screenshot);
    }

    [Fact]
    public void ShouldDeleteScreenshot()
    {
        ScreenshotsLibrary library = new();
        var screenshot = library.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        library.DeleteScreenshot(screenshot);
        library.Screenshots.Should().NotContain(screenshot);
    }

    [Fact]
    public void HasAnyScreenshotsShouldBeTrueWhenAddScreenshot()
    {
        ScreenshotsLibrary library = new();
        library.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        library.HasAnyScreenshots.Should().BeTrue();
    }

    [Fact]
    public void HasAnyScreenshotsShouldBeFalseWhenAddThenDeleteScreenshot()
    {
        ScreenshotsLibrary library = new();
        var screenshot = library.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        library.DeleteScreenshot(screenshot);
        library.HasAnyScreenshots.Should().BeFalse();
    }
}