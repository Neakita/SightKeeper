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

	[Fact]
	public void ShouldNotDeleteAssetByImageThatWasNotCreated()
	{
		var image = Utilities.CreateImage();
		var library = CreateLibrary();
		Assert.Throws<ArgumentException>(() => library.DeleteAsset(image));
	}

	[Fact]
	public void ShouldNotDeleteAssetByImageTwice()
	{
		var image = Utilities.CreateImage();
		var library = CreateLibrary();
		library.MakeAsset(image);
		library.DeleteAsset(image);
		Assert.Throws<ArgumentException>(() => library.DeleteAsset(image));
	}

	[Fact]
	public void ShouldGetNewAsset()
	{
		var image = Utilities.CreateImage();
		var library = CreateLibrary();
		var asset = library.GetOrMakeAsset(image);
		library.Assets.Should().Contain(asset);
	}

	[Fact]
	public void ShouldGetExistingAsset()
	{
		var image = Utilities.CreateImage();
		var library = CreateLibrary();
		var asset = library.MakeAsset(image);
		var asset2 = library.GetOrMakeAsset(image);
		library.Assets.Should().HaveCount(1);
		asset2.Should().Be(asset);
	}

	[Fact]
	public void ShouldContainImageAfterAssetCreation()
	{
		var image = Utilities.CreateImage();
		var library = CreateLibrary();
		library.MakeAsset(image);
		library.Contains(image).Should().BeTrue();
	}

	[Fact]
	public void ShouldNotContainImageForWhichAssetWasNotCreated()
	{
		var image = Utilities.CreateImage();
		var library = CreateLibrary();
		library.Contains(image).Should().BeFalse();
	}

	[Fact]
	public void ShouldNotGetOptionalAssetForImageWhichAssetWasNotCreated()
	{
		var image = Utilities.CreateImage();
		var library = CreateLibrary();
		library.GetOptionalAsset(image).Should().BeNull();
	}

	[Fact]
	public void ShouldGetOptionalAsset()
	{
		var image = Utilities.CreateImage();
		var library = CreateLibrary();
		var asset = library.MakeAsset(image);
		library.GetOptionalAsset(image).Should().Be(asset);
	}

	private static AssetsOwner<Asset> CreateLibrary()
	{
		var dataSet = CreateDataSet();
		return dataSet.AssetsLibrary;
	}

	private static DataSet CreateDataSet()
	{
		return new DomainDetectorDataSet();
	}
}