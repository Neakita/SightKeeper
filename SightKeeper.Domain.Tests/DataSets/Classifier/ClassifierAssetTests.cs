using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierAssetTests
{
	[Fact]
	public void ShouldNotSetTagToForeign()
	{
		ClassifierDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.Tag.Should().Be(tag);
		Assert.ThrowsAny<Exception>(() => asset.Tag = new ClassifierDataSet().Tags.CreateTag(""));
		asset.Tag.Should().Be(tag);
	}
}