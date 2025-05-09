using FluentAssertions;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Services;

public sealed class AppDataBoundingAnnotatorTests
{
	[Fact]
	public void ShouldCreateAssetWithItem()
	{
		var annotator = PrepareAnnotator();
		var (dataSet, tag) = PrepareDataSet();
		var screenshot = PrepareScreenshot();
		var item = annotator.CreateItem(dataSet.AssetsLibrary, screenshot, tag, new Bounding());
		dataSet.AssetsLibrary.Assets.Should().Contain(asset => asset.Items.Contains(item));
	}

	[Fact]
	public void ShouldCreateItemInExistingAsset()
	{
		var annotator = PrepareAnnotator();
		var (dataSet, tag) = PrepareDataSet();
		var screenshot = PrepareScreenshot();
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item = annotator.CreateItem(dataSet.AssetsLibrary, screenshot, tag, new Bounding());
		asset.Items.Single().Should().Be(item);
	}

	private static AppDataBoundingAnnotator PrepareAnnotator()
	{
		AppDataAccess dataAccess = new();
		Lock appDataLock = new();
		AppDataBoundingAnnotator annotator = new(appDataLock, dataAccess);
		return annotator;
	}

	private static (DetectorDataSet dataSet, Tag tag) PrepareDataSet()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		return (dataSet, tag);
	}

	private static Image PrepareScreenshot()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		return screenshot;
	}
}