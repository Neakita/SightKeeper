using NSubstitute;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class DomainPoserTagTests
{
	[Fact]
	public void ShouldAllowKeyPointTagCreation()
	{
		var innerTag = Substitute.For<PoserTag>();
		var tag = new DomainPoserTag(innerTag);
		const string keyPointTagName = "KeyPoint";
		tag.CreateKeyPointTag(keyPointTagName);
		innerTag.Received().CreateKeyPointTag(keyPointTagName);
	}

	[Fact]
	public void ShouldAllowKeyPointTagCreationWhenTagHasAssociatedItems()
	{
		var innerTag = Substitute.For<PoserTag>();
		innerTag.Users.Returns([Substitute.For<TagUser>()]);
		var tag = new DomainPoserTag(innerTag);
		const string keyPointTagName = "KeyPoint";
		tag.CreateKeyPointTag(keyPointTagName);
		innerTag.Received().CreateKeyPointTag(keyPointTagName);
	}

	[Fact]
	public void ShouldAllowKeyPointTagDeletionWithoutAssociatedUsers()
	{
		var innerTag = Substitute.For<PoserTag>();
		innerTag.Users.Returns([Substitute.For<TagUser>()]);
		var keyPointTag = Substitute.For<Tag>();
		var tag = new DomainPoserTag(innerTag);
		tag.DeleteKeyPointTag(keyPointTag);
		innerTag.Received().DeleteKeyPointTag(keyPointTag);
	}

	[Fact]
	public void ShouldDisallowKeyPointDeletionWithAssociatedUsers()
	{
		var innerTag = Substitute.For<PoserTag>();
		var keyPointTag = Substitute.For<Tag>();
		keyPointTag.Users.Returns([Substitute.For<TagUser>()]);
		var tag = new DomainPoserTag(innerTag);
		Assert.Throws<TagIsInUseException>(() => tag.DeleteKeyPointTag(keyPointTag));
		innerTag.DidNotReceive().DeleteKeyPointTag(Arg.Any<Tag>());
	}

	[Fact]
	public void ShouldDeleteKeyPointTagByIndex()
	{
		var innerTag = Substitute.For<PoserTag>();
		var keyPointTag = Substitute.For<Tag>();
		innerTag.KeyPointTags.Returns([keyPointTag]);
		var tag = new DomainPoserTag(innerTag);
		tag.DeleteKeyPointTagAt(0);
		innerTag.Received().DeleteKeyPointTagAt(0);
	}

	[Fact]
	public void ShouldCreateKeyPointTagViaTagsOwner()
	{
		var innerTag = Substitute.For<PoserTag>();
		var tag = new DomainPoserTag(innerTag);
		TagsOwner<Tag> tagAsTagsOwner = tag;
		const string keyPointTagName = "keyPointTag";
		tagAsTagsOwner.CreateTag(keyPointTagName);
		innerTag.Received().CreateKeyPointTag(keyPointTagName);
	}

	[Fact]
	public void ShouldDeleteKeyPointTagByIndexViaTagsOwner()
	{
		var innerTag = Substitute.For<PoserTag>();
		var keyPointTag = Substitute.For<Tag>();
		innerTag.KeyPointTags.Returns([keyPointTag]);
		var tag = new DomainPoserTag(innerTag);
		TagsOwner<Tag> tagAsTagsOwner = tag;
		tagAsTagsOwner.DeleteTagAt(0);
		innerTag.Received().DeleteKeyPointTagAt(0);
	}
}