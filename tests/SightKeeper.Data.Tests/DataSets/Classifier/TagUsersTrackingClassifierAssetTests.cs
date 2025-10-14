using NSubstitute;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Tests.DataSets.Classifier;

public sealed class TagUsersTrackingClassifierAssetTests
{
	[Fact]
	public void ShouldRemoveOldTagUser()
	{
		var innerAsset = Substitute.For<ClassifierAsset>();
		var oldTag = Substitute.For<Tag, EditableTagUsers>();
		innerAsset.Tag.Returns(oldTag);
		var tagUser = Substitute.For<TagUser>();
		var asset = new TagUsersTrackingClassifierAsset(innerAsset)
		{
			TagUser = tagUser
		};
		asset.Tag = Substitute.For<Tag, EditableTagUsers>();
		((EditableTagUsers)oldTag).Received().RemoveUser(tagUser);
	}

	[Fact]
	public void ShouldAddNewTagUser()
	{
		var innerAsset = Substitute.For<ClassifierAsset>();
		var tagUser = Substitute.For<TagUser>();
		var asset = new TagUsersTrackingClassifierAsset(innerAsset)
		{
			TagUser = tagUser
		};
		asset.Tag.Returns(Substitute.For<Tag, EditableTagUsers>());
		var newTag = Substitute.For<Tag, EditableTagUsers>();
		asset.Tag = newTag;
		((EditableTagUsers)newTag).Received().AddUser(tagUser);
	}

	[Fact]
	public void ShouldSetTagInInnerSet()
	{
		var innerAsset = Substitute.For<ClassifierAsset>();
		innerAsset.Tag.Returns(Substitute.For<Tag, EditableTagUsers>());
		var tagUser = Substitute.For<TagUser>();
		var asset = new TagUsersTrackingClassifierAsset(innerAsset)
		{
			TagUser = tagUser
		};
		var newTag = Substitute.For<Tag, EditableTagUsers>();
		asset.Tag = newTag;
		innerAsset.Received().Tag = newTag;
	}
}