using FluentAssertions;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests;

public sealed class WeightsLibraryTests
{
    [Fact]
    public void ShouldCreateScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        dataSet.Screenshots.Screenshots.Should().Contain(screenshot);
    }

    [Fact]
    public void ShouldDeleteScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        dataSet.Screenshots.DeleteScreenshot(screenshot);
        dataSet.Screenshots.Screenshots.Should().NotContain(screenshot);
    }

    [Fact]
    public void HasAnyScreenshotsShouldBeTrueWhenAddScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        dataSet.Screenshots.HasAnyScreenshots.Should().BeTrue();
    }

    [Fact]
    public void HasAnyScreenshotsShouldBeFalseWhenAddThenDeleteScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        dataSet.Screenshots.DeleteScreenshot(screenshot);
        dataSet.Screenshots.HasAnyScreenshots.Should().BeFalse();
    }
}