using FluentAssertions;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Tests.Annotation.ToolBars;

public sealed class ClassifierToolBarTests
{
	[Fact]
	public void TagsShouldBeEmptyByDefault()
	{
		ToolBar.Tags.Should().BeEmpty();
	}
	
	[Fact]
	public void ShouldSetTagsFromDataSet()
	{
		var toolBar = ToolBar;
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("Tag1");
		var tag2 = dataSet.TagsLibrary.CreateTag("Tag2");
		toolBar.DataSet = dataSet;
		toolBar.Tags.Should().Contain([tag1, tag2]);
	}

	[Fact]
	public void ShouldNotifyTagsChangeWhenDataSetChanged()
	{
		var subjectToolBar = ToolBar;
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("Tag");
		using var monitoredToolBar = subjectToolBar.Monitor();
		subjectToolBar.DataSet = dataSet;
		monitoredToolBar.Should().RaisePropertyChangeFor(toolBar => toolBar.Tags);
	}

	[Fact]
	public void ShouldNotSetTagWithoutDataSet()
	{
		var toolBar = ToolBar;
		var tag = new ClassifierDataSet().TagsLibrary.CreateTag("TestTag");
		Assert.ThrowsAny<Exception>(() => toolBar.Tag = tag);
	}

	[Fact]
	public void ShouldNotSetTagWithoutScreenshot()
	{
		var toolBar = ToolBar;
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		toolBar.DataSet = dataSet;
		Assert.ThrowsAny<Exception>(() => toolBar.Tag = tag);
	}

	[Fact]
	public void ShouldNotSetTagWithDifferentDataSet()
	{
		var toolBar = ToolBar;
		ClassifierDataSet dataSet = new();
		var tag = new ClassifierDataSet().TagsLibrary.CreateTag("TestTag");
		toolBar.DataSet = dataSet;
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		toolBar.Screenshot = screenshot;
		Assert.ThrowsAny<Exception>(() => toolBar.Tag = tag);
	}

	[Fact]
	public void ShouldSetTag()
	{
		var toolBar = ToolBar;
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		toolBar.DataSet = dataSet;
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		toolBar.Screenshot = screenshot;
		toolBar.Tag = tag;
		dataSet.AssetsLibrary.Assets.Should().ContainKey(screenshot).WhoseValue.Tag.Should().Be(tag);
	}

	[Fact]
	public void ShouldSetTagFromScreenshot()
	{
		var toolBar = ToolBar;
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.Tag = tag;
		toolBar.DataSet = dataSet;
		toolBar.Screenshot = screenshot;
		toolBar.Tag.Should().Be(tag);
	}

	[Fact]
	public void ShouldNotifyTagChangeWhenScreenshotSet()
	{
		var subjectToolBar = ToolBar;
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.Tag = tag;
		subjectToolBar.DataSet = dataSet;
		using var monitoredToolBar = subjectToolBar.Monitor();
		subjectToolBar.Screenshot = screenshot;
		monitoredToolBar.Should().RaisePropertyChangeFor(toolBar => toolBar.Tag);
	}

	private static ClassifierToolBarViewModel ToolBar => new Composition().ClassifierAnnotationContext.ToolBar;
}