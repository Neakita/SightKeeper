using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.Images;

public sealed class ImageSetTests
{
	[Fact]
	public void ShouldNotDeleteImageWithAsset()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTime.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(image);
		Assert.ThrowsAny<Exception>(() => imageSet.RemoveImageAt(0));
	}

	[Fact]
	public void ShouldDeleteImageAfterAssetDeletion()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTime.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(image);
		dataSet.AssetsLibrary.DeleteAsset(image);
		imageSet.RemoveImageAt(0);
	}

	[Fact]
	public void ShouldDeleteImageAfterAssetsLibraryClear()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTime.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(image);
		dataSet.AssetsLibrary.ClearAssets();
		imageSet.RemoveImageAt(0);
	}
}