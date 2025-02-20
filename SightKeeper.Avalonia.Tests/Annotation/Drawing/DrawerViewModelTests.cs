using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Tests.Annotation.Drawing;

public sealed class DrawerViewModelTests
{
	[Fact]
	public void ShouldSetBoundingDrawerViewModelTag()
	{
		var drawerViewModel = CreateDrawerViewModel(out var boundingDrawerViewModel);
		var tag = CreateTag();
		drawerViewModel.Tag = tag;
		boundingDrawerViewModel.Tag.Should().Be(tag);
	}

	private static DrawerViewModel CreateDrawerViewModel(out BoundingDrawerViewModel boundingDrawerViewModel)
	{
		boundingDrawerViewModel = new BoundingDrawerViewModel(Substitute.For<BoundingAnnotator>());
		var drawerItemsFactory = new DrawerItemsFactory(Substitute.For<BoundingEditor>());
		var keyPointViewModelFactory = new KeyPointViewModelFactory(Substitute.For<PoserAnnotator>());
		var assetItemsViewModel = new AssetItemsViewModel(
			drawerItemsFactory,
			keyPointViewModelFactory,
			Substitute.For<ObservableBoundingAnnotator>(),
			Substitute.For<ObservablePoserAnnotator>());
		var keyPointDrawerViewModel = new KeyPointDrawerViewModel(Substitute.For<PoserAnnotator>());
		DrawerViewModel drawerViewModel = new(boundingDrawerViewModel, assetItemsViewModel, keyPointDrawerViewModel);
		return drawerViewModel;
	}

	private static Tag CreateTag()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		return tag;
	}
}