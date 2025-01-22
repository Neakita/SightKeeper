using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Tests.Annotation.Drawing;

public sealed class Poser3DDrawerViewModelTests
{
	[Fact]
	public void ShouldNotHaveItemsByDefault()
	{
		var drawer = CreateDrawer();
		drawer.Items.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotHaveSelectedTagByDefault()
	{
		var drawer = CreateDrawer();
		drawer.Tag.Should().BeNull();
	}

	[Fact]
	public void ShouldHaveItemsFromScreenshotAssociatedAsset()
	{
		var screenshot = CreateScreenshot();
		Poser3DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, Bounding);
		var drawer = CreateDrawer();
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		drawer.Items.Should().Contain(itemViewModel => itemViewModel.Value == item);
	}

	[Fact]
	public void ShouldCreateItem()
	{
		var screenshot = CreateScreenshot();
		Poser3DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var drawer = CreateDrawer();
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
		var screenshot = CreateScreenshot();
		Poser3DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var drawer = CreateDrawer();
		drawer.Screenshot = screenshot;
		drawer.SetTag(tag);
		Assert.ThrowsAny<Exception>(() => drawer.CreateItemCommand.Execute(Bounding));
	}

	[Fact]
	public void ShouldNotCreateItemWithoutScreenshot()
	{
		Poser3DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var drawer = CreateDrawer();
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.SetTag(tag);
		Assert.ThrowsAny<Exception>(() => drawer.CreateItemCommand.Execute(Bounding));
	}

	[Fact]
	public void ShouldNotCreateItemWithoutTag()
	{
		var screenshot = CreateScreenshot();
		Poser3DDataSet dataSet = new();
		var drawer = CreateDrawer();
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		Assert.ThrowsAny<Exception>(() => drawer.CreateItemCommand.Execute(Bounding));
	}

	[Fact]
	public void ShouldClearItemsAfterChangingAssetsLibrary()
	{
		var screenshot = CreateScreenshot();
		Poser3DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag, Bounding);
		var drawer = CreateDrawer();
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		drawer.AssetsLibrary = new Poser3DDataSet().AssetsLibrary;
		drawer.Items.Should().BeEmpty();
	}

	[Fact]
	public void ShouldClearItemsAfterChangingScreenshot()
	{
		var screenshot = CreateScreenshot();
		Poser3DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag, Bounding);
		var drawer = CreateDrawer();
		drawer.AssetsLibrary = dataSet.AssetsLibrary;
		drawer.Screenshot = screenshot;
		drawer.Screenshot = CreateScreenshot();
		drawer.Items.Should().BeEmpty();
	}

	private static Vector2<ushort> ImageSize => new(320, 320);
	private static Bounding Bounding => new(0.1, 0.1, 0.9, 0.9);

	private static Screenshot CreateScreenshot()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, ImageSize);
		return screenshot;
	}

	private static Poser3DDrawerViewModel CreateDrawer() => new Composition().Poser3DDrawerViewModel;
}