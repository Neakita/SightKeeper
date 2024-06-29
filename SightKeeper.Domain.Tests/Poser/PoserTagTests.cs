﻿using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Tests.Poser;

public class PoserTagTests
{
	[Fact]
	public void ShouldNotChangeTagNameToOccupied()
	{
		PoserDataSet dataSet = new("", 320);
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		Assert.ThrowsAny<Exception>(() => tag2.Name = "1");
		tag1.Name.Should().Be("1");
		tag2.Name.Should().Be("2");
	}

	[Fact]
	public void ShouldSetTagNameToDeletedTagName()
	{
		PoserDataSet dataSet = new("", 320);
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		dataSet.Tags.DeleteTag(tag1);
		tag2.Name = tag1.Name;
	}

	[Fact]
	public void ShouldNotAddNewPointToTagWithAssociatedItems()
	{
		PoserDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding(), []);
		Assert.ThrowsAny<Exception>(() => tag.AddKeyPoint(""));
		tag.KeyPoints.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeletePointOfTagWithAssociatedItems()
	{
		PoserDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		var keyPoint = tag.AddKeyPoint("");
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding(), [new Vector2<double>()]);
		Assert.ThrowsAny<Exception>(() => tag.DeleteKeyPoint(keyPoint));
		tag.KeyPoints.Should().Contain(keyPoint);
	}

	[Fact]
	public void ShouldAddNewPointToTagWithoutAssociatedItems()
	{
		PoserDataSet dataSet = new("", 320);
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag1, new Bounding(), []);
		var keyPoint = tag2.AddKeyPoint("");
		tag2.KeyPoints.Should().Contain(keyPoint);
	}

	[Fact]
	public void ShouldDeletePointOfTagWithoutAssociatedItems()
	{
		PoserDataSet dataSet = new("", 320);
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		var keyPoint1 = tag1.AddKeyPoint("");
		var keyPoint2 = tag2.AddKeyPoint("");
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag1, new Bounding(), [new Vector2<double>()]);
		tag2.DeleteKeyPoint(keyPoint2);
		tag2.KeyPoints.Should().BeEmpty();
	}
}