using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.Annotation;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
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

	private static BoundedItem CreateItem()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var screenshot = CreateScreenshot();
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		return asset.CreateItem(tag, new Bounding(0.1, 0.2, 0.3, 0.4));
	}

	private static Image CreateScreenshot()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		return screenshot;
	}
}