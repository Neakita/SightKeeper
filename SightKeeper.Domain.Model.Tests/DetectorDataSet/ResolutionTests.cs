using FluentAssertions;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class ResolutionTests
{
    [Fact]
    public void ShouldChangeResolutionWhenNoScreenshotsAndAssets()
    {
        const ushort firstResolution = 64;
        const ushort secondResolution = 128;
        DataSet.DataSet dataSet = new("Test data set", firstResolution);
        dataSet.Resolution.Should().Be(firstResolution);
        dataSet.Resolution = secondResolution;
        dataSet.Resolution.Should().Be(secondResolution);
    }
    
    [Fact]
    public void ShouldThrowExceptionOnResolutionChangeWhenHaveScreenshots()
    {
        const ushort firstResolution = 64;
        const ushort secondResolution = 128;
        DataSet.DataSet dataSet = new("Test data set", firstResolution);
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
        const ushort firstResolution = 64;
        const ushort secondResolution = 128;
        DataSet.DataSet dataSet = new("Test data set", firstResolution);
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