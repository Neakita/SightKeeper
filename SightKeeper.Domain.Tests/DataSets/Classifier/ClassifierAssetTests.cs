using FluentAssertions;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierAssetTests
{
	[Fact]
	public void ShouldNotSetTagToForeign()
	{
		ClassifierDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.Tag.Should().Be(tag);
		Assert.ThrowsAny<Exception>(() => asset.Tag = new ClassifierDataSet("", 320).Tags.CreateTag(""));
		asset.Tag.Should().Be(tag);
	}
}