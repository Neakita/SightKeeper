using System.Windows.Input;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Tests.Annotation.Drawing;

public sealed class BoundingDrawerViewModelTests
{
	[Fact]
	public void ShouldCallBoundingAnnotator()
	{
		var boundingAnnotator = CreateBoundingAnnotator();
		var (assetsLibrary, tag) = CreateDataSetWithTag();
		var screenshot = CreateScreenshot();
		var drawerViewModel = CreateBoundingDrawerViewModel(boundingAnnotator, assetsLibrary, tag, screenshot);
		Bounding bounding = new(0.1, 0.2, 0.3, 0.4);
		drawerViewModel.CreateItemCommand.Execute(bounding);
		boundingAnnotator.Received().CreateItem(assetsLibrary, screenshot, tag, bounding);
	}

	[Fact]
	public void CanExecuteShouldBeTrue()
	{
		var boundingAnnotator = CreateBoundingAnnotator();
		var (assetsLibrary, tag) = CreateDataSetWithTag();
		var screenshot = CreateScreenshot();
		var drawerViewModel = CreateBoundingDrawerViewModel(boundingAnnotator, assetsLibrary, tag, screenshot);
		drawerViewModel.CreateItemCommand.CanExecute(default).Should().BeTrue();
	}

	[Fact]
	public void CanExecuteShouldBeFalseWhenNoAssetsLibraryProvided()
	{
		var boundingAnnotator = CreateBoundingAnnotator();
		var (_, tag) = CreateDataSetWithTag();
		var screenshot = CreateScreenshot();
		var drawerViewModel = CreateBoundingDrawerViewModel(boundingAnnotator, null, tag, screenshot);
		drawerViewModel.CreateItemCommand.CanExecute(default).Should().BeFalse();
	}

	[Fact]
	public void CanExecuteShouldBeFalseWhenNoTagProvided()
	{
		var boundingAnnotator = CreateBoundingAnnotator();
		var (assetsLibrary, _) = CreateDataSetWithTag();
		var screenshot = CreateScreenshot();
		var drawerViewModel = CreateBoundingDrawerViewModel(boundingAnnotator, assetsLibrary, null, screenshot);
		drawerViewModel.CreateItemCommand.CanExecute(default).Should().BeFalse();
	}

	[Fact]
	public void CanExecuteShouldBeFalseWhenNoScreenshotProvided()
	{
		var boundingAnnotator = CreateBoundingAnnotator();
		var (assetsLibrary, tag) = CreateDataSetWithTag();
		var drawerViewModel = CreateBoundingDrawerViewModel(boundingAnnotator, assetsLibrary, tag, null);
		drawerViewModel.CreateItemCommand.CanExecute(default).Should().BeFalse();
	}

	[Fact]
	public void CanExecuteShouldBeFalseWhenNothingProvided()
	{
		var boundingAnnotator = CreateBoundingAnnotator();
		var drawerViewModel = CreateBoundingDrawerViewModel(boundingAnnotator, null, null, null);
		drawerViewModel.CreateItemCommand.CanExecute(default).Should().BeFalse();
	}

	[Fact]
	public void CanExecuteShouldBecomeTrueWhenLastNullPropertyFilled()
	{
		var boundingAnnotator = CreateBoundingAnnotator();
		var (assetsLibrary, tag) = CreateDataSetWithTag();
		var screenshot = CreateScreenshot();
		var drawerViewModel = CreateBoundingDrawerViewModel(boundingAnnotator, assetsLibrary, tag, null);
		drawerViewModel.Image = screenshot;
		drawerViewModel.CreateItemCommand.CanExecute(default).Should().BeTrue();
	}

	[Fact]
	public void ShouldRaiseCanExecuteChangedEvent()
	{
		var boundingAnnotator = CreateBoundingAnnotator();
		var (assetsLibrary, tag) = CreateDataSetWithTag();
		var screenshot = CreateScreenshot();
		var drawerViewModel = CreateBoundingDrawerViewModel(boundingAnnotator, assetsLibrary, tag, null);
		using var commandMonitor = drawerViewModel.CreateItemCommand.Monitor();
		drawerViewModel.Image = screenshot;
		commandMonitor.Should().Raise(nameof(ICommand.CanExecuteChanged));
	}

	private static BoundingAnnotator CreateBoundingAnnotator()
	{
		return Substitute.For<BoundingAnnotator>();
	}

	private static (AssetsOwner<ItemsMaker<AssetItem>> assetsLibrary, DomainTag tag) CreateDataSetWithTag()
	{
		DomainDetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		return (dataSet.AssetsLibrary, tag);
	}

	private static DomainImage CreateScreenshot()
	{
		DomainImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		return screenshot;
	}

	private static BoundingDrawerViewModel CreateBoundingDrawerViewModel(
		BoundingAnnotator boundingAnnotator,
		AssetsOwner<ItemsMaker<AssetItem>>? assetsLibrary,
		DomainTag? tag,
		DomainImage? screenshot)
	{
		BoundingDrawerViewModel drawerViewModel = new(boundingAnnotator)
		{
			AssetsLibrary = assetsLibrary,
			Tag = tag,
			Image = screenshot
		};
		return drawerViewModel;
	}
}