using System.Reactive.Linq;
using FluentAssertions;
using NSubstitute;
using Serilog.Core;
using SightKeeper.Application.Annotation;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Drawing.Bounded;
using SightKeeper.Avalonia.Annotation.Drawing.Poser;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

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
		var observableBoundingAnnotator = Substitute.For<ObservableBoundingAnnotator>();
		observableBoundingAnnotator.ItemCreated.Returns(Observable.Empty<(ItemsMaker<AssetItem>, AssetItem)>());
		var observablePoserAnnotator = Substitute.For<ObservablePoserAnnotator>();
		observablePoserAnnotator.KeyPointCreated.Returns(Observable.Empty<(PoserItem, KeyPoint)>());
		observablePoserAnnotator.KeyPointDeleted.Returns(Observable.Empty<(PoserItem, KeyPoint)>());
		var observableAnnotator = Substitute.For<ObservableAnnotator>();
		observableAnnotator.AssetsChanged.Returns(Observable.Empty<Image>());
		var assetItemsViewModel = new AssetItemsViewModel(
			drawerItemsFactory,
			keyPointViewModelFactory,
			observableBoundingAnnotator,
			observablePoserAnnotator,
			observableAnnotator);
		var keyPointDrawerViewModel = new KeyPointDrawerViewModel(Substitute.For<PoserAnnotator>());
		DrawerViewModel drawerViewModel = new(boundingDrawerViewModel, assetItemsViewModel, keyPointDrawerViewModel, new ImageLoader(new WriteableBitmapPool(Logger.None), Substitute.For<ReadImageDataAccess>()));
		return drawerViewModel;
	}

	private static Tag CreateTag()
	{
		DomainDetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		return tag;
	}
}