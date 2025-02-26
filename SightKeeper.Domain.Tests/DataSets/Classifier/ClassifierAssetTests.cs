using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierAssetTests
{
	[Fact]
	public void ShouldNotSetTagToForeign()
	{
		ImageSet imageSet = new();
		var screenshot = imageSet.CreateImage(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.Tag.Should().Be(tag);
		Assert.ThrowsAny<Exception>(() => asset.Tag = new ClassifierDataSet().TagsLibrary.CreateTag(""));
		asset.Tag.Should().Be(tag);
	}
}