using NSubstitute;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.DataSets.Classifier;

public sealed class TagUsersTrackingClassifierAssetsLibraryTests
{
	[Fact]
	public void ShouldAddTagUserToTagWhenMakingAsset()
	{
		var innerLibrary = Substitute.For<AssetsOwner<ClassifierAsset>>();
		var library = new TagUsersTrackingClassifierAssetsLibrary(innerLibrary);
		var asset = Substitute.For<ClassifierAsset>();
		var tag = Substitute.For<Tag, EditableTagUsers>();
		asset.Tag.Returns(tag);
		innerLibrary.MakeAsset(Arg.Any<ManagedImage>()).Returns(asset);
		library.MakeAsset(Substitute.For<ManagedImage>());
		((EditableTagUsers)tag).Received().AddUser(asset);
	}

	[Fact]
	public void ShouldRemoveTagUserFromTagWhenRemovingAsset()
	{
		var innerLibrary = Substitute.For<AssetsOwner<ClassifierAsset>>();
		var library = new TagUsersTrackingClassifierAssetsLibrary(innerLibrary);
		var asset = Substitute.For<ClassifierAsset>();
		var tag = Substitute.For<Tag, EditableTagUsers>();
		innerLibrary.GetAsset(Arg.Any<ManagedImage>()).Returns(asset);
		asset.Tag.Returns(tag);
		library.DeleteAsset(Substitute.For<ManagedImage>());
		((EditableTagUsers)tag).Received().RemoveUser(asset);
	}

	[Fact]
	public void ShouldRemoveTagUsersFromTagsWhenClearingAssets()
	{
		var innerLibrary = Substitute.For<AssetsOwner<ClassifierAsset>>();
		var library = new TagUsersTrackingClassifierAssetsLibrary(innerLibrary);
		var tag1 = Substitute.For<Tag, EditableTagUsers>();
		var tag2 = Substitute.For<Tag, EditableTagUsers>();
		var asset1 = Substitute.For<ClassifierAsset>();
		var asset2 = Substitute.For<ClassifierAsset>();
		var asset3 = Substitute.For<ClassifierAsset>();
		asset1.Tag.Returns(tag1);
		asset2.Tag.Returns(tag2);
		asset3.Tag.Returns(tag2);
		innerLibrary.Assets.Returns([asset1, asset2, asset3]);
		library.ClearAssets();
		((EditableTagUsers)tag1).Received().RemoveUser(asset1);
		((EditableTagUsers)tag2).Received().RemoveUser(asset2);
		((EditableTagUsers)tag2).Received().RemoveUser(asset3);
	}
}