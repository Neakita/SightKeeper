using FluentAssertions;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class AssetsLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		var image = Utilities.CreateImage();
		var library = CreateLibrary();
		var asset = library.MakeAsset(image);
		library.Assets.Should().Contain(asset).Which.Image.Should().Be(image);
	}

	[Fact]
	public void ShouldNotCreateAssetForSameImageTwice()
	{
		var image = Utilities.CreateImage();
		var library = CreateLibrary();
		var asset = library.MakeAsset(image);
		Assert.Throws<ArgumentException>(() => library.MakeAsset(image));
		library.Assets.Should().Contain(asset).And.HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		var image = Utilities.CreateImage();
		var library = CreateLibrary();
		library.MakeAsset(image);
		library.DeleteAsset(image);
		library.Assets.Should().BeEmpty();
	}

	private static AssetsLibrary CreateLibrary()
	{
		var dataSet = CreateDataSet();
		return dataSet.AssetsLibrary;
	}

	private static DataSet CreateDataSet()
	{
		return new DetectorDataSet();
	}
}