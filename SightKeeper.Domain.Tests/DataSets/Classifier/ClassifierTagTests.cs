using FluentAssertions;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public class ClassifierTagTests
{
	[Fact]
	public void ShouldNotChangeTagNameToOccupied()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		Assert.ThrowsAny<Exception>(() => tag2.Name = "1");
		tag1.Name.Should().Be("1");
		tag2.Name.Should().Be("2");
	}

	[Fact]
	public void ShouldSetTagNameToDeletedTagName()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		dataSet.Tags.DeleteTag(tag1);
		tag2.Name = tag1.Name;
	}
}