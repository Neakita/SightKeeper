using NSubstitute;
using SightKeeper.Application.Annotating;
using SightKeeper.Tests.Common;

namespace SightKeeper.Application.Tests;

public sealed class DataSetScreenshoterTests
{
    [Fact]
    public void ShouldSetDataSet()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenCapture = Substitute.For<ScreenCapture>();
        Screenshoter screenshoter = new(screenCapture, Substitute.For<SelfActivityService>());
        DataSetScreenshoter dataSetScreenshoter = new(screenCapture, screenshoter, Substitute.For<GamesService>());
        dataSetScreenshoter.DataSet = dataSet;
        dataSetScreenshoter.DataSet.Should().Be(dataSet);
    }
    
    [Fact]
    public void ShouldSetDataSetToNull()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenCapture = Substitute.For<ScreenCapture>();
        var selfActivity = Substitute.For<SelfActivityService>();
        var gamesService = Substitute.For<GamesService>();
        Screenshoter screenshoter = new(screenCapture, selfActivity);
        DataSetScreenshoter dataSetScreenshoter = new(screenCapture, screenshoter, gamesService);
        dataSetScreenshoter.DataSet = dataSet;
        dataSetScreenshoter.DataSet = null;
        dataSetScreenshoter.DataSet.Should().BeNull();
    }
}