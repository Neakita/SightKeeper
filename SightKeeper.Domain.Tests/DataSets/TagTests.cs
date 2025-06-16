using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class TagTests
{
	[Fact]
	public void ShouldAllowSetTagName()
	{
		var innerTag = Substitute.For<Tag>();
		const string newName = "Tag2";
		var domainTag = new DomainTag(innerTag);
		domainTag.Name = newName;
		innerTag.Received().Name = newName;
	}

	[Fact]
	public void ShouldNotChangeTagNameToOccupied()
	{
		var innerTag = Substitute.For<Tag>();
		var otherTag = Substitute.For<Tag>();
		const string conflictingName = "Tag2";
		otherTag.Name.Returns(conflictingName);
		innerTag.Owner.Tags.Returns([otherTag]);
		var domainTag = new DomainTag(innerTag);
		var exception = Assert.Throws<TagsConflictingNameException>(() => domainTag.Name = conflictingName);
		innerTag.DidNotReceive().Name = Arg.Any<string>();
		exception.ConflictingTag.Should().Be(otherTag);
		exception.EditingTag.Should().Be(domainTag);
		exception.Name.Should().Be(conflictingName);
	}
}