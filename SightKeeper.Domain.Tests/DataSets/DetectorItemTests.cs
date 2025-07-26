using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets;

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

	[Fact]
	public void ShouldAllowSetNormalizedBounding()
	{
		var innerItem = Substitute.For<DetectorItem>();
		var domainItem = new DomainDetectorItem(innerItem);
		var bounding = new Bounding(0.2, 0.3, 0.4, 0.5);
		domainItem.Bounding = bounding;
		innerItem.Received().Bounding = bounding;
	}

	[Fact]
	public void ShouldDisallowSetNotNormalizedBounding()
	{
		var innerItem = Substitute.For<DetectorItem>();
		var domainItem = new DomainDetectorItem(innerItem);
		var bounding = new Bounding(2, 3, 4, 5);
		var exception = Assert.Throws<ItemBoundingConstraintException>(() => domainItem.Bounding = bounding);
		innerItem.DidNotReceive().Bounding = Arg.Any<Bounding>();
		exception.Item.Should().Be(domainItem);
		exception.Value.Should().Be(bounding);
	}
}