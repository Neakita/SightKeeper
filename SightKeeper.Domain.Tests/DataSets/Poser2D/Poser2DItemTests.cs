using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DItemTests
{
	[Fact]
	public void ShouldClearKeyPointsWhenChangingTag()
	{
		var initialTag = Substitute.For<PoserTag>();
		var newTag = Substitute.For<PoserTag>();
		var tagsOwner = Substitute.For<TagsOwner<PoserTag>>();
		// some weird NSubstitute & interface default implementation behavior
		((Tag)initialTag).Owner.Returns(tagsOwner);
		((Tag)newTag).Owner.Returns(tagsOwner);
		var innerItem = Substitute.For<Poser2DItem>();
		innerItem.Tag.Returns(initialTag);
		DomainPoser2DItem domainItem = new(innerItem);
		domainItem.Tag = newTag;
		innerItem.Received().ClearKeyPoints();
	}

	[Fact]
	public void ShouldAllowSetBounding()
	{
		var innerItem = Substitute.For<Poser2DItem>();
		DomainPoser2DItem domainItem = new(innerItem);
		var bounding = new Bounding(.1, .2, .3, .4);
		domainItem.Bounding = bounding;
		innerItem.Received().Bounding = bounding;
	}

	[Fact]
	public void ShouldDisallowSetNotNormalizedBounding()
	{
		var innerItem = Substitute.For<Poser2DItem>();
		DomainPoser2DItem domainItem = new(innerItem);
		var bounding = new Bounding(1, 2, 3, 4);
		var exception = Assert.Throws<ItemBoundingConstraintException>(() => domainItem.Bounding = bounding);
		innerItem.DidNotReceive().Bounding = Arg.Any<Bounding>();
		exception.Item.Should().Be(domainItem);
		exception.Value.Should().Be(bounding);
	}

	[Fact]
	public void ShouldAllowMakeKeyPoint()
	{
		var innerItem = Substitute.For<Poser2DItem>();
		DomainPoser2DItem domainItem = new(innerItem);
		var poserTag = Substitute.For<PoserTag>();
		var keyPointTag = Substitute.For<Tag>();
		keyPointTag.Owner.Returns(poserTag);
		innerItem.Tag.Returns(poserTag);
		var expectedKeyPoint = Substitute.For<KeyPoint>();
		innerItem.MakeKeyPoint(keyPointTag).Returns(expectedKeyPoint);
		var keyPoint = domainItem.MakeKeyPoint(keyPointTag);
		keyPoint.Should().BeSameAs(expectedKeyPoint);
		innerItem.Received().MakeKeyPoint(keyPointTag);
	}

	[Fact]
	public void ShouldDisallowMakeKeyPointWithDifferentTagOwner()
	{
		var innerItem = Substitute.For<Poser2DItem>();
		DomainPoser2DItem domainItem = new(innerItem);
		var poserTag = Substitute.For<PoserTag>();
		var keyPointTag = Substitute.For<Tag>();
		innerItem.Tag.Returns(poserTag);
		var expectedKeyPoint = Substitute.For<KeyPoint>();
		innerItem.MakeKeyPoint(keyPointTag).Returns(expectedKeyPoint);
		var exception = Assert.Throws<UnexpectedTagsOwnerException>(() => domainItem.MakeKeyPoint(keyPointTag));
		innerItem.DidNotReceive().MakeKeyPoint(Arg.Any<Tag>());
		exception.ExpectedOwner.Should().Be(poserTag);
		exception.Causer.Should().Be(keyPointTag);
	}
}