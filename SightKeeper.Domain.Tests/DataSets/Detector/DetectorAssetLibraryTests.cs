using FluentAssertions;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		dataSet.AssetsLibrary.Assets.Should().ContainValue(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		Assert.ThrowsAny<Exception>(() => dataSet.AssetsLibrary.MakeAsset(image));
		dataSet.AssetsLibrary.Assets.Should().ContainValue(asset);
		dataSet.AssetsLibrary.Assets.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		dataSet.AssetsLibrary.MakeAsset(image);
		dataSet.AssetsLibrary.DeleteAsset(image);
		dataSet.AssetsLibrary.Assets.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet1 = new();
		DetectorDataSet dataSet2 = new();
		dataSet1.AssetsLibrary.MakeAsset(image);
		Assert.ThrowsAny<Exception>(() => dataSet2.AssetsLibrary.DeleteAsset(image));
	}
}