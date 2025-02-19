using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing;
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
		DrawerViewModel drawerViewModel = new(boundingDrawerViewModel,
			new AssetItemsViewModel(new DrawerItemsFactory(Substitute.For<BoundingEditor>()),
				Substitute.For<ObservableBoundingAnnotator>()));
		return drawerViewModel;
	}

	private static Tag CreateTag()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		return tag;
	}
}