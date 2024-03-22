using FluentAssertions;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests;

public sealed class ScreenshotsLibraryTests
{
    [Fact]
    public void ShouldCreateScreenshot()
    {
	    SimpleScreenshotsDataAccess screenshotsDataAccess = new();
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        dataSet.Screenshots.Should().Contain(screenshot);
    }

    [Fact]
    public void ShouldDeleteScreenshot()
    {
	    SimpleScreenshotsDataAccess screenshotsDataAccess = new();
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        dataSet.Screenshots.DeleteScreenshot(screenshot);
        dataSet.Screenshots.Should().NotContain(screenshot);
    }
}