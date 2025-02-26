using FluentAssertions;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Services;

public sealed class AppDataBoundingEditorTests
{
	[Fact]
	public void ShouldChangeBounding()
	{
		var annotator = PrepareBoundingEditor();
		var item = PrepareItem();
		Bounding newBounding = new(0.1, 0.2, 0.3, 0.4);
		annotator.SetBounding(item, newBounding);
		item.Bounding.Should().Be(newBounding);
	}

	private static AppDataBoundingEditor PrepareBoundingEditor()
	{
		AppDataAccess dataAccess = new();
		Lock appDataLock = new();
		AppDataBoundingEditor annotator = new(appDataLock, dataAccess);
		return annotator;
	}

	private static BoundedItem PrepareItem()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("TestTag");
		var screenshot = PrepareScreenshot();
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		return asset.CreateItem(tag, new Bounding());
	}

	private static Image PrepareScreenshot()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateScreenshot(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
		return screenshot;
	}
}