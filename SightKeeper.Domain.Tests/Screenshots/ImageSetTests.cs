using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.Screenshots;

public sealed class ImageSetTests
{
	[Fact]
	public void ShouldNotDeleteScreenshotWithAsset()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTime.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => imageSet.RemoveImageAt(0));
	}

	[Fact]
	public void ShouldDeleteScreenshotAfterAssetDeletion()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTime.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.DeleteAsset(screenshot);
		imageSet.RemoveImageAt(0);
	}

	[Fact]
	public void ShouldDeleteScreenshotAfterAssetsLibraryClear()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTime.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.ClearAssets();
		imageSet.RemoveImageAt(0);
	}
}