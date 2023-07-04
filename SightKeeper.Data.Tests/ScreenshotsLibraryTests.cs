using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Data.Tests;

public sealed class ScreenshotsLibraryTests
{
    [Fact]
    public void ShouldAddScreenshotFromLibrary()
    {
        ScreenshotsLibrary screenshotsLibrary = new();
        var screenshot = screenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        DetectorModel model = new("Test model");
        var asset = model.MakeAssetFromScreenshot(screenshot);
        model.Assets.Should().Contain(asset);
    }
}