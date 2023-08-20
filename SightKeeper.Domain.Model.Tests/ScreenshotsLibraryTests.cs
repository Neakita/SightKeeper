using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests;

public sealed class ScreenshotsLibraryTests
{
    [Fact]
    public void ShouldCreateScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        dataSet.ScreenshotsLibrary.Screenshots.Should().Contain(screenshot);
    }

    [Fact]
    public void ShouldDeleteScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        dataSet.ScreenshotsLibrary.DeleteScreenshot(screenshot);
        dataSet.ScreenshotsLibrary.Screenshots.Should().NotContain(screenshot);
    }

    [Fact]
    public void HasAnyScreenshotsShouldBeTrueWhenAddScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        dataSet.ScreenshotsLibrary.HasAnyScreenshots.Should().BeTrue();
    }

    [Fact]
    public void HasAnyScreenshotsShouldBeFalseWhenAddThenDeleteScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        dataSet.ScreenshotsLibrary.DeleteScreenshot(screenshot);
        dataSet.ScreenshotsLibrary.HasAnyScreenshots.Should().BeFalse();
    }
}