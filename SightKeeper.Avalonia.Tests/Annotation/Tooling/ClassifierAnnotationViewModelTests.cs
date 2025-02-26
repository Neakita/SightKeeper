using FluentAssertions;
using SightKeeper.Avalonia.Annotation.Tooling;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Tests.Annotation.Tooling;

public sealed class ClassifierAnnotationViewModelTests
{
	[Fact]
	public void TagsShouldBeEmptyByDefault()
	{
		Annotation.Tags.Should().BeEmpty();
	}
	
	[Fact]
	public void ShouldSetTagsFromDataSet()
	{
		var sideBar = Annotation;
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("Tag1");
		var tag2 = dataSet.TagsLibrary.CreateTag("Tag2");
		sideBar.DataSet = dataSet;
		sideBar.Tags.Should().Contain([tag1, tag2]);
	}

	[Fact]
	public void ShouldNotifyTagsChangeWhenDataSetChanged()
	{
		var subjectSideBar = Annotation;
		ClassifierDataSet dataSet = new();
		dataSet.TagsLibrary.CreateTag("Tag");
		using var monitoredSideBar = subjectSideBar.Monitor();
		subjectSideBar.DataSet = dataSet;
		monitoredSideBar.Should().RaisePropertyChangeFor(sideBar => sideBar.Tags);
	}

	[Fact]
	public void ShouldNotSetTagWithoutDataSet()
	{
		var sideBar = Annotation;
		var tag = new ClassifierDataSet().TagsLibrary.CreateTag("TestTag");
		Assert.ThrowsAny<Exception>(() => sideBar.SelectedTag = tag);
	}

	[Fact]
	public void ShouldNotSetTagWithoutScreenshot()
	{
		var sideBar = Annotation;
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		sideBar.DataSet = dataSet;
		Assert.ThrowsAny<Exception>(() => sideBar.SelectedTag = tag);
	}

	[Fact]
	public void ShouldNotSetTagWithDifferentDataSet()
	{
		var sideBar = Annotation;
		ClassifierDataSet dataSet = new();
		var tag = new ClassifierDataSet().TagsLibrary.CreateTag("TestTag");
		sideBar.DataSet = dataSet;
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		sideBar.Screenshot = screenshot;
		Assert.ThrowsAny<Exception>(() => sideBar.SelectedTag = tag);
	}

	[Fact]
	public void ShouldSetTag()
	{
		var sideBar = Annotation;
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		sideBar.DataSet = dataSet;
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		sideBar.Screenshot = screenshot;
		sideBar.SelectedTag = tag;
		dataSet.AssetsLibrary.Assets.Should().ContainKey(screenshot).WhoseValue.Tag.Should().Be(tag);
	}

	[Fact]
	public void ShouldSetTagFromScreenshot()
	{
		var sideBar = Annotation;
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.Tag = tag;
		sideBar.DataSet = dataSet;
		sideBar.Screenshot = screenshot;
		sideBar.SelectedTag.Should().Be(tag);
	}

	[Fact]
	public void ShouldNotifyTagChangeWhenScreenshotSet()
	{
		var subjectSideBar = Annotation;
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.Tag = tag;
		subjectSideBar.DataSet = dataSet;
		using var monitoredSideBar = subjectSideBar.Monitor();
		subjectSideBar.Screenshot = screenshot;
		monitoredSideBar.Should().RaisePropertyChangeFor(sideBar => sideBar.SelectedTag);
	}

	private static ClassifierAnnotationViewModel Annotation => new Composition().ClassifierAnnotationViewModel;
}