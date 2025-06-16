/*using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class TagsLibraryTests
{
	[Fact]
	public void ShouldCreateTag()
	{
		var library = Utilities.CreateTagsLibrary();
		var tag = library.CreateTag("");
		library.Tags.Should().Contain(tag);
	}

	[Fact]
	public void ShouldCreateMultipleTags()
	{
		DomainClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var tag3 = dataSet.TagsLibrary.CreateTag("3");
		dataSet.TagsLibrary.Tags.Should().Contain([tag1, tag2, tag3]);
	}

	[Fact]
	public void ShouldDeleteTag()
	{
		DomainClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		dataSet.TagsLibrary.DeleteTag(tag);
		dataSet.TagsLibrary.Tags.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateTagWithOccupiedName()
	{
		DomainClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		Assert.ThrowsAny<Exception>(() => dataSet.TagsLibrary.CreateTag("1"));
		dataSet.TagsLibrary.Tags.Should().Contain(tag1);
		dataSet.TagsLibrary.Tags.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldNotDeleteTagTwice()
	{
		var library = Utilities.CreateTagsLibrary();
		var tag = library.CreateTag("");
		library.DeleteTag(tag);
		Assert.Throws<ArgumentException>(() => library.DeleteTag(tag));
	}

	[Fact]
	public void ShouldDeleteTagByIndex()
	{
		var library = Utilities.CreateTagsLibrary();
		library.CreateTag("1");
		library.CreateTag("2");
		var tag = library.CreateTag("3");
		library.DeleteTagAt(2);
		library.Tags.Should().NotContain(tag);
	}
}*/