using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.Tests.Annotation.Drawing;

public sealed class DrawerItemsFactoryTests
{
	[Fact]
	public void ShouldCreateDetectorItemViewModel()
	{
		DrawerItemsFactory factory = new(Substitute.For<BoundingEditor>());
		var item = CreateItem();
		var itemViewModel = factory.CreateItemViewModel(item);
		itemViewModel.Should().BeOfType<DetectorItemViewModel>();
	}

	private static AssetItem CreateItem()
	{
		DomainDetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var screenshot = CreateScreenshot();
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		return asset.MakeItem(tag);
	}

	private static DomainImage CreateScreenshot()
	{
		DomainImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		return screenshot;
	}
}