using FluentAssertions;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests;

public sealed class ScreenshotsLibraryTests
{
    [Fact]
    public void ShouldCreateScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        dataSet.ScreenshotsLibrary.Screenshots.Should().Contain(screenshot);
    }

    [Fact]
    public void ShouldDeleteScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        dataSet.ScreenshotsLibrary.DeleteScreenshot(screenshot);
        dataSet.ScreenshotsLibrary.Screenshots.Should().NotContain(screenshot);
    }

    [Fact]
    public void HasAnyScreenshotsShouldBeTrueWhenAddScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        dataSet.ScreenshotsLibrary.HasAnyScreenshots.Should().BeTrue();
    }

    [Fact]
    public void HasAnyScreenshotsShouldBeFalseWhenAddThenDeleteScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        dataSet.ScreenshotsLibrary.DeleteScreenshot(screenshot);
        dataSet.ScreenshotsLibrary.HasAnyScreenshots.Should().BeFalse();
    }
}