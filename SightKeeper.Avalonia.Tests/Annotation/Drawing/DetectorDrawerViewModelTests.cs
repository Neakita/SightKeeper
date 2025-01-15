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
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, new Bounding(0.1, 0.1, 0.9, 0.9));
		var drawer = Drawer;
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		drawer.Items.Should().Contain(itemViewModel => itemViewModel.Value == item);
	}

	[Fact]
	public void ShouldCreateItem()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var drawer = Drawer;
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		drawer.SetTag(tag);
		Bounding bounding = new(0.1, 0.1, 0.9, 0.9);
		drawer.CreateItemCommand.Execute(bounding);
		drawer.Items.Should().Contain(itemViewModel => itemViewModel.Bounding == bounding);
		dataSet.AssetsLibrary.Assets.Single().Value.Items.Should().Contain(item => item.Bounding == bounding);
	}

	[Fact]
	public void ShouldNotCreateItemWithoutAssetsLibrary()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var drawer = Drawer;
		drawer.Screenshot = screenshot;
		drawer.SetTag(tag);
		Bounding bounding = new(0.1, 0.1, 0.9, 0.9);
		Assert.ThrowsAny<Exception>(() => drawer.CreateItemCommand.Execute(bounding));
	}

	[Fact]
	public void ShouldNotCreateItemWithoutScreenshot()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var drawer = Drawer;
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.SetTag(tag);
		Bounding bounding = new(0.1, 0.1, 0.9, 0.9);
		Assert.ThrowsAny<Exception>(() => drawer.CreateItemCommand.Execute(bounding));
	}

	[Fact]
	public void ShouldNotCreateItemWithoutTag()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var drawer = Drawer;
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		Bounding bounding = new(0.1, 0.1, 0.9, 0.9);
		Assert.ThrowsAny<Exception>(() => drawer.CreateItemCommand.Execute(bounding));
	}

	[Fact]
	public void ShouldClearItemsAfterChangingAssetsLibrary()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding(0.1, 0.1, 0.9, 0.9));
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
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding(0.1, 0.1, 0.9, 0.9));
		var drawer = Drawer;
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		drawer.Screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		drawer.Items.Should().BeEmpty();
	}

	private static DetectorDrawerViewModel Drawer => new Composition().DetectorAnnotationContext.Drawer;
}