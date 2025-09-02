using NSubstitute;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Tests.DataSets.Classifier;

public sealed class TagUsersTrackingClassifierAssetTests
{
	[Fact]
	public void ShouldRemoveOldTagUser()
	{
		var innerAsset = Substitute.For<StorableClassifierAsset>();
		var oldTag = Substitute.For<StorableTag>();
		innerAsset.Tag.Returns(oldTag);
		var tagUser = Substitute.For<TagUser>();
		var asset = new TagUsersTrackingClassifierAsset(innerAsset)
		{
			TagUser = tagUser
		};
		asset.Tag = Substitute.For<StorableTag>();
		oldTag.Received().RemoveUser(tagUser);
	}

	[Fact]
	public void ShouldAddNewTagUser()
	{
		var innerAsset = Substitute.For<StorableClassifierAsset>();
		var tagUser = Substitute.For<TagUser>();
		var asset = new TagUsersTrackingClassifierAsset(innerAsset)
		{
			TagUser = tagUser
		};
		var newTag = Substitute.For<StorableTag>();
		asset.Tag = newTag;
		newTag.Received().AddUser(tagUser);
	}

	[Fact]
	public void ShouldSetTagInInnerSet()
	{
		var innerAsset = Substitute.For<StorableClassifierAsset>();
		var tagUser = Substitute.For<TagUser>();
		var asset = new TagUsersTrackingClassifierAsset(innerAsset)
		{
			TagUser = tagUser
		};
		var newTag = Substitute.For<StorableTag>();
		asset.Tag = newTag;
		innerAsset.Received().Tag = newTag;
	}
}