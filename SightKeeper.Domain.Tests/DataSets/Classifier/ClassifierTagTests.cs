using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public class ClassifierTagTests
{
	[Fact]
	public void ShouldNotChangeTagNameToOccupied()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var exception = Assert.Throws<TagsConflictingNameException>(() => tag2.Name = "1");
		tag1.Name.Should().Be("1");
		tag2.Name.Should().Be("2");
		exception.ConflictingTag.Should().Be(tag1);
		exception.EditingTag.Should().Be(tag2);
		exception.Name.Should().Be("1");
	}

	[Fact]
	public void ShouldSetTagNameToDeletedTagName()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		dataSet.TagsLibrary.DeleteTag(tag1);
		tag2.Name = tag1.Name;
	}
}