using NSubstitute;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets.Decorators;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data.Tests.DataSets.Classifier;

public sealed class TagUsersTrackingClassifierAssetsLibraryTests
{
	[Fact]
	public void ShouldAddTagUserToTagWhenMakingAsset()
	{
		var innerLibrary = Substitute.For<StorableAssetsOwner<StorableClassifierAsset>>();
		var library = new TagUsersTrackingClassifierAssetsLibrary(innerLibrary);
		var asset = Substitute.For<StorableClassifierAsset>();
		var tag = Substitute.For<StorableTag>();
		asset.Tag.Returns(tag);
		innerLibrary.MakeAsset(Arg.Any<StorableImage>()).Returns(asset);
		library.MakeAsset(Substitute.For<StorableImage>());
		tag.Received().AddUser(asset);
	}

	[Fact]
	public void ShouldRemoveTagUserFromTagWhenRemovingAsset()
	{
		var innerLibrary = Substitute.For<StorableAssetsOwner<StorableClassifierAsset>>();
		var library = new TagUsersTrackingClassifierAssetsLibrary(innerLibrary);
		var asset = Substitute.For<StorableClassifierAsset>();
		var tag = Substitute.For<StorableTag>();
		innerLibrary.GetAsset(Arg.Any<StorableImage>()).Returns(asset);
		asset.Tag.Returns(tag);
		library.DeleteAsset(Substitute.For<StorableImage>());
		tag.Received().RemoveUser(asset);
	}

	[Fact]
	public void ShouldRemoveTagUsersFromTagsWhenClearingAssets()
	{
		var innerLibrary = Substitute.For<StorableAssetsOwner<StorableClassifierAsset>>();
		var library = new TagUsersTrackingClassifierAssetsLibrary(innerLibrary);
		var tag1 = Substitute.For<StorableTag>();
		var tag2 = Substitute.For<StorableTag>();
		var asset1 = Substitute.For<StorableClassifierAsset>();
		var asset2 = Substitute.For<StorableClassifierAsset>();
		var asset3 = Substitute.For<StorableClassifierAsset>();
		asset1.Tag.Returns(tag1);
		asset2.Tag.Returns(tag2);
		asset3.Tag.Returns(tag2);
		innerLibrary.Assets.Returns([asset1, asset2, asset3]);
		library.ClearAssets();
		tag1.Received().RemoveUser(asset1);
		tag2.Received().RemoveUser(asset2);
		tag2.Received().RemoveUser(asset3);
	}
}