﻿using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorAssetTests
{
	[Fact]
	public void ShouldCreateItem()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.CreateItem(tag, new Bounding());
		asset.Items.Should().Contain(item);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameTag()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item1 = asset.CreateItem(tag, new Bounding(0, 0, 0.5, 0.5));
		var item2 = asset.CreateItem(tag, new Bounding(0, 0, 1, 1));
		asset.Items.Should().Contain([item1, item2]);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameBounding()
	{
		ImageSet imageSet = new();
		var image = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item1 = asset.CreateItem(tag1, new Bounding());
		var item2 = asset.CreateItem(tag2, new Bounding());
		asset.Items.Should().Contain([item1, item2]);
	}
}