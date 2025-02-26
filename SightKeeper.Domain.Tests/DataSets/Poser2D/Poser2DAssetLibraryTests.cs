using FluentAssertions;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet = new();
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.Assets.Should().ContainValue(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet = new();
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet.AssetsLibrary.MakeAsset(screenshot));
		dataSet.AssetsLibrary.Assets.Should().ContainValue(asset);
		dataSet.AssetsLibrary.Assets.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(screenshot);
		dataSet.AssetsLibrary.DeleteAsset(screenshot);
		dataSet.AssetsLibrary.Assets.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		Poser2DDataSet dataSet1 = new();
		Poser2DDataSet dataSet2 = new();
		dataSet1.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet2.AssetsLibrary.DeleteAsset(screenshot));
	}
}