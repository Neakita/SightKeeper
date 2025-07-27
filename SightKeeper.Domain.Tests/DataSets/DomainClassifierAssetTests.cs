using FluentAssertions;
using NSubstitute;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class DomainClassifierAssetTests
{
	[Fact]
	public void ShouldAllowUpdateTag()
	{
		var innerAsset = Substitute.For<ClassifierAsset>();
		var domainAsset = new DomainClassifierAsset(innerAsset);
		var tagsOwner = Substitute.For<TagsOwner<Tag>>();
		var tag1 = Substitute.For<Tag>();
		tag1.Owner.Returns(tagsOwner);
		innerAsset.Tag.Returns(tag1);
		var tag2 = Substitute.For<Tag>();
		tag2.Owner.Returns(tagsOwner);
		domainAsset.Tag = tag2;
		innerAsset.Received().Tag = tag2;
	}

	[Fact]
	public void ShouldNotAllowSetTagWithDifferentOwner()
	{
		var initialTag = Substitute.For<Tag>();
		initialTag.Owner.Returns(Substitute.For<TagsOwner<Tag>>());
		var newTag = Substitute.For<Tag>();
		newTag.Owner.Returns(Substitute.For<TagsOwner<Tag>>());
		var innerAsset = Substitute.For<ClassifierAsset>();
		innerAsset.Tag.Returns(initialTag);
		var domainAsset = new DomainClassifierAsset(innerAsset);
		var exception = Assert.Throws<InappropriateTagsOwnerChangeException>(() => domainAsset.Tag = newTag);
		domainAsset.Tag.Should().Be(initialTag);
		exception.Causer.Should().Be(newTag);
		exception.CurrentTag.Should().Be(initialTag);
	}
}