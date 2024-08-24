﻿using FluentAssertions;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierTagsLibraryTests
{
	[Fact]
	public void ShouldCreateTag()
	{
		ClassifierDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		dataSet.Tags.Should().Contain(tag);
	}

	[Fact]
	public void ShouldCreateMultipleTags()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		var tag3 = dataSet.Tags.CreateTag("3");
		dataSet.Tags.Should().Contain([tag1, tag2, tag3]);
	}

	[Fact]
	public void ShouldNotCreateTagWithOccupiedName()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		Assert.ThrowsAny<Exception>(() => dataSet.Tags.CreateTag("1"));
		dataSet.Tags.Should().Contain(tag1);
		dataSet.Tags.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteTag()
	{
		ClassifierDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		dataSet.Tags.DeleteTag(tag);
		dataSet.Tags.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteTagWithAssociatedAsset()
	{
		ClassifierDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		var screenshot = dataSet.Screenshots.AddScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => dataSet.Tags.DeleteTag(tag));
		dataSet.Tags.Should().Contain(tag);
		dataSet.Assets.Should().Contain(asset);
		asset.Tag.Should().Be(tag);
	}
}