using FluentAssertions;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class ResolutionTests
{
    [Fact]
    public void ShouldChangeResolutionWhenNoScreenshotsAndAssets()
    {
        Resolution firstResolution = new(64, 64);
        Resolution secondResolution = new(128, 128);
        DataSet dataSet = new("Test data set", firstResolution);
        dataSet.Resolution.Should().Be(firstResolution);
        dataSet.Resolution = secondResolution;
        dataSet.Resolution.Should().Be(secondResolution);
    }
    
    [Fact]
    public void ShouldThrowExceptionOnResolutionChangeWhenHaveScreenshots()
    {
        Resolution firstResolution = new(64, 64);
        Resolution secondResolution = new(128, 128);
        DataSet dataSet = new("Test data set", firstResolution);
        dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        dataSet.Resolution.Should().Be(firstResolution);
        Assert.Throws<InvalidOperationException>(() =>
        {
            dataSet.Resolution = secondResolution;
        });
        dataSet.Resolution.Should().Be(firstResolution);
    }
    
    [Fact]
    public void ShouldThrowExceptionOnResolutionChangeWhenHaveAssets()
    {
        Resolution firstResolution = new(64, 64);
        Resolution secondResolution = new(128, 128);
        DataSet dataSet = new("Test data set", firstResolution);
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        dataSet.MakeAsset(screenshot);
        dataSet.Resolution.Should().Be(firstResolution);
        Assert.Throws<InvalidOperationException>(() =>
        {
            dataSet.Resolution = secondResolution;
        });
        dataSet.Resolution.Should().Be(firstResolution);
    }
}