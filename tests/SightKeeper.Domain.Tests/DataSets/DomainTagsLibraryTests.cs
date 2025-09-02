using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class DomainTagsLibraryTests
{
	[Fact]
	public void ShouldCreateTag()
	{
		const string tagName = "Tag1";
		var innerLibrary = Substitute.For<TagsOwner<Tag>>();
		var expectedTag = Substitute.For<Tag>();
		innerLibrary.CreateTag(tagName).Returns(expectedTag);
		DomainTagsLibrary<Tag> domainLibrary = new(innerLibrary);
		var tag = domainLibrary.CreateTag(tagName);
		tag.Should().BeSameAs(expectedTag);
		innerLibrary.Received().CreateTag(tagName);
	}

	[Fact]
	public void ShouldAllowDeleteTag()
	{
		var innerLibrary = Substitute.For<TagsOwner<Tag>>();
		var tag = Substitute.For<Tag>();
		innerLibrary.Tags.Returns([tag]);
		DomainTagsLibrary<Tag> domainLibrary = new(innerLibrary);
		domainLibrary.DeleteTagAt(0);
		innerLibrary.Received().DeleteTagAt(0);
	}

	[Fact]
	public void ShouldNotAllowCreateTagWithOccupiedName()
	{
		const string tagName = "Tag1";
		var innerLibrary = Substitute.For<TagsOwner<Tag>>();
		var tag = Substitute.For<Tag>();
		tag.Name.Returns(tagName);
		innerLibrary.Tags.Returns([tag]);
		DomainTagsLibrary<Tag> domainLibrary = new(innerLibrary);
		var exception = Assert.Throws<ArgumentException>(() => domainLibrary.CreateTag(tagName));
		exception.ParamName.Should().Be("name");
		innerLibrary.DidNotReceive().CreateTag(Arg.Any<string>());
	}

	[Fact]
	public void ShouldNotAllowDeleteTagWithUsers()
	{
		var innerLibrary = Substitute.For<TagsOwner<Tag>>();
		var tag = Substitute.For<Tag>();
		tag.Users.Returns([Substitute.For<TagUser>()]);
		innerLibrary.Tags.Returns([tag]);
		DomainTagsLibrary<Tag> domainLibrary = new(innerLibrary);
		var exception = Assert.Throws<TagIsInUseException>(() => domainLibrary.DeleteTagAt(0));
		innerLibrary.DidNotReceive().DeleteTagAt(Arg.Any<int>());
		exception.Tag.Should().BeSameAs(tag);
	}
}