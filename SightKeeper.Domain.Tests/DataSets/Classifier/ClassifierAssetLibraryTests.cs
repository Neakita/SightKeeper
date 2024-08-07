﻿using FluentAssertions;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierAssetLibraryTests
{
	[Fact]
	public void ShouldCreateAsset()
	{
		ClassifierDataSet dataSet = new("", 320);
		dataSet.Tags.CreateTag("");
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
	}

	[Fact]
	public void ShouldNotCreateDuplicateAsset()
	{
		ClassifierDataSet dataSet = new("", 320);
		dataSet.Tags.CreateTag("");
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet.Assets.MakeAsset(screenshot));
		screenshot.Asset.Should().Be(asset);
		dataSet.Assets.Should().Contain(asset);
		dataSet.Assets.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteAsset()
	{
		ClassifierDataSet dataSet = new("", 320);
		dataSet.Tags.CreateTag("");
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		dataSet.Assets.DeleteAsset(asset);
		dataSet.Assets.Should().BeEmpty();
		screenshot.Asset.Should().BeNull();
	}

	[Fact]
	public void ShouldNotDeleteAssetFromOtherDataSet()
	{
		ClassifierDataSet dataSet1 = new("", 320);
		dataSet1.Tags.CreateTag("");
		ClassifierDataSet dataSet2 = new("", 320);
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet1.Screenshots, []);
		var asset = dataSet1.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet2.Assets.DeleteAsset(asset));
		asset.Screenshot.Asset.Should().Be(asset);
	}
	
	[Fact]
	public void ShouldNotSetAssetTagToForeign()
	{
		ClassifierDataSet dataSet = new("", 320);
		var properTag = dataSet.Tags.CreateTag("");
		var foreignTag = new ClassifierDataSet("", 320).Tags.CreateTag("");
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => asset.Tag = foreignTag);
		asset.Tag.Should().Be(properTag);
	}
	
	[Fact]
	public void ShouldNotCreateAssetWithoutAvailableTags()
	{
		ClassifierDataSet dataSet = new("", 320);
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		Assert.ThrowsAny<Exception>(() => dataSet.Assets.MakeAsset(screenshot));
	}
}