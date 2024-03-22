using FluentAssertions;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Tests;

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

    [Fact]
    public void ShouldProperlyRemoveExceed()
    {
	    SimpleScreenshotsDataAccess screenshotsDataAccess = new();
	    var dataSet = DomainTestsHelper.NewDataSet;
	    dataSet.Screenshots.MaxQuantity = 2;
	    var screenshot1 = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
	    var screenshot2 = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
	    dataSet.Screenshots.Should().ContainInOrder([screenshot1, screenshot2]);
	    var screenshot3 = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
	    dataSet.Screenshots.Should().ContainInOrder([screenshot2, screenshot3]);
	    var screenshot4 = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
	    dataSet.Screenshots.Should().ContainInOrder([screenshot3, screenshot4]);
	    
    }
}