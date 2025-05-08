using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets.Tags;

public sealed class TagsLibraryTests
{
	[Fact]
	public void ShouldNotDeleteTagTwice()
	{
		var library = CreateTagsLibrary();
		var tag = library.CreateTag("");
		library.DeleteTag(tag);
		Assert.Throws<ArgumentException>(() => library.DeleteTag(tag));
	}

	[Fact]
	public void ShouldDeleteTagByIndex()
	{
		var library = CreateTagsLibrary();
		library.CreateTag("1");
		library.CreateTag("2");
		var tag = library.CreateTag("3");
		library.DeleteTagAt(2);
		library.Tags.Should().NotContain(tag);
	}

	private static TagsLibrary<Tag> CreateTagsLibrary()
	{
		return new ClassifierDataSet().TagsLibrary;
	}
}