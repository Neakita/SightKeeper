using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorItemTests
{
	[Fact]
	public void ShouldAllowUpdateTag()
	{
		var innerItem = Substitute.For<DetectorItem>();
		var domainItem = new DomainDetectorItem(innerItem);
		var initialTag = Substitute.For<Tag>();
		innerItem.Tag.Returns(initialTag);
		var newTag = Substitute.For<Tag>();
		var tagsOwner = Substitute.For<TagsOwner<Tag>>();
		initialTag.Owner.Returns(tagsOwner);
		newTag.Owner.Returns(tagsOwner);
		domainItem.Tag = newTag;
		innerItem.Received().Tag = newTag;
	}

	[Fact]
	public void ShouldNotAllowSetTagWithDifferentOwner()
	{
		var innerItem = Substitute.For<DetectorItem>();
		var domainItem = new DomainDetectorItem(innerItem);
		var initialTag = Substitute.For<Tag>();
		innerItem.Tag.Returns(initialTag);
		var newTag = Substitute.For<Tag>();
		initialTag.Owner.Returns(Substitute.For<TagsOwner<Tag>>());
		newTag.Owner.Returns(Substitute.For<TagsOwner<Tag>>());
		var exception = Assert.Throws<InappropriateTagsOwnerChangeException>(() => domainItem.Tag = newTag);
		innerItem.DidNotReceive().Tag = Arg.Any<Tag>();
		exception.Causer.Should().Be(newTag);
		exception.CurrentTag.Should().Be(initialTag);
	}
}