using FluentAssertions;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets.Tags;

public sealed class TagTests
{
	[Fact]
	public void ShouldNotChangeTagNameToOccupied()
	{
		var library = Utilities.CreateTagsLibrary();
		var tag1 = library.CreateTag("1");
		var tag2 = library.CreateTag("2");
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
		var library = Utilities.CreateTagsLibrary();
		var tag1 = library.CreateTag("1");
		var tag2 = library.CreateTag("2");
		library.DeleteTag(tag1);
		tag2.Name = tag1.Name;
	}
}