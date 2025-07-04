﻿using FluentAssertions;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DAssetTests
{
	[Fact]
	public void ShouldCreateKeyPoints()
	{
		var image = Utilities.CreateImage();
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var firstKeyPointTag = tag.CreateKeyPointTag("1");
		var secondKeyPointTag = tag.CreateKeyPointTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag);
		var firstKeyPoint = item.MakeKeyPoint(firstKeyPointTag);
		var secondKeyPoint = item.MakeKeyPoint(secondKeyPointTag);
		asset.Items.Should().Contain(item);
		item.KeyPoints.Should().Contain(firstKeyPoint).And.Contain(secondKeyPoint);
	}

	[Fact]
	public void ShouldDeleteKeyPoint()
	{
		var image = Utilities.CreateImage();
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var keyPointTag = tag.CreateKeyPointTag("1");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag);
		var keyPoint = item.MakeKeyPoint(keyPointTag);
		item.DeleteKeyPoint(keyPoint);
		item.KeyPoints.Should().NotContain(keyPoint);
	}

	[Fact]
	public void ShouldNotDeleteKeyPointTwice()
	{
		var image = Utilities.CreateImage();
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var keyPointTag = tag.CreateKeyPointTag("1");
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag);
		var keyPoint = item.MakeKeyPoint(keyPointTag);
		item.DeleteKeyPoint(keyPoint);
		Assert.Throws<ArgumentException>(() => item.DeleteKeyPoint(keyPoint));
	}
}