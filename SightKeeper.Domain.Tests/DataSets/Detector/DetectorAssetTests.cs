﻿using FluentAssertions;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorAssetTests
{
	[Fact]
	public void ShouldCreateItem()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, new Bounding());
		asset.Items.Should().Contain(item);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameTag()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		var item1 = asset.CreateItem(tag, new Bounding(0, 0, 0.5, 0.5));
		var item2 = asset.CreateItem(tag, new Bounding(0, 0, 1, 1));
		asset.Items.Should().Contain([item1, item2]);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameBounding()
	{
		DetectorDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		var item1 = asset.CreateItem(tag1, new Bounding());
		var item2 = asset.CreateItem(tag2, new Bounding());
		asset.Items.Should().Contain([item1, item2]);
	}
}