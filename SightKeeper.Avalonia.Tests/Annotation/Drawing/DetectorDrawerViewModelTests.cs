using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Tests.Annotation.Drawing;

public sealed class DetectorDrawerViewModelTests
{
	[Fact]
	public void ShouldNotHaveItemsByDefault()
	{
		Drawer.Items.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotHaveSelectedTagByDefault()
	{
		Drawer.Tag.Should().BeNull();
	}

	[Fact]
	public void ShouldHaveItemsFromScreenshotAssociatedAsset()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, ImageSize);
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, Bounding);
		var drawer = Drawer;
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		drawer.Items.Should().Contain(itemViewModel => itemViewModel.Value == item);
	}

	[Fact]
	public void ShouldCreateItem()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, ImageSize);
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var drawer = Drawer;
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		drawer.SetTag(tag);
		drawer.CreateItemCommand.Execute(Bounding);
		drawer.Items.Should().Contain(itemViewModel => itemViewModel.Bounding == Bounding);
		dataSet.AssetsLibrary.Assets.Single().Value.Items.Should().Contain(item => item.Bounding == Bounding);
	}

	[Fact]
	public void ShouldNotCreateItemWithoutAssetsLibrary()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, ImageSize);
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var drawer = Drawer;
		drawer.Screenshot = screenshot;
		drawer.SetTag(tag);
		Assert.ThrowsAny<Exception>(() => drawer.CreateItemCommand.Execute(Bounding));
	}

	[Fact]
	public void ShouldNotCreateItemWithoutScreenshot()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var drawer = Drawer;
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.SetTag(tag);
		Assert.ThrowsAny<Exception>(() => drawer.CreateItemCommand.Execute(Bounding));
	}

	[Fact]
	public void ShouldNotCreateItemWithoutTag()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, ImageSize);
		DetectorDataSet dataSet = new();
		var drawer = Drawer;
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		Assert.ThrowsAny<Exception>(() => drawer.CreateItemCommand.Execute(Bounding));
	}

	[Fact]
	public void ShouldClearItemsAfterChangingAssetsLibrary()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, ImageSize);
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag, Bounding);
		var drawer = Drawer;
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		drawer.AssetsLibrary = new DetectorDataSet().AssetsLibrary;
		drawer.Items.Should().BeEmpty();
	}

	[Fact]
	public void ShouldClearItemsAfterChangingScreenshot()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, ImageSize);
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag, Bounding);
		var drawer = Drawer;
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		drawer.Screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, ImageSize);
		drawer.Items.Should().BeEmpty();
	}

	private static Vector2<ushort> ImageSize => new(320, 320);
	private static Bounding Bounding => new(0.1, 0.1, 0.9, 0.9);
	private static DetectorDrawerViewModel Drawer => new Composition().DetectorAnnotationContext.Drawer;
}