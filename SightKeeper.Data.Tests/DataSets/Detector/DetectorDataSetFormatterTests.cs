using FlakeId;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data.Tests.DataSets.Detector;

public sealed class DetectorDataSetFormatterTests
{
	[Fact]
	public void ShouldPersistItemTag()
	{
		var set = Substitute.For<StorableDetectorDataSet>();
		var tag = Substitute.For<StorableTag>();
		set.TagsLibrary.Tags.Returns([tag]);
		var image = new InMemoryImage(Id.Create(), default, default);
		var asset = Substitute.For<StorableItemsAsset<StorableDetectorItem>>();
		var item = Substitute.For<StorableDetectorItem>();
		set.AssetsLibrary.Assets.Returns([asset]);
		asset.Items.Returns([item]);
		item.Tag.Returns(tag);
		asset.Image.Returns(image);
		var persistedSet = set.PersistUsingFormatter(Formatter);
		var persistedTag = persistedSet.TagsLibrary.Tags.Single();
		persistedSet.AssetsLibrary.Assets.Should().ContainSingle().Which.Items.Should().ContainSingle().Which.Tag.Should().Be(persistedTag);
	}

	[Fact]
	public void ShouldPersistTagUser()
	{
		var set = Substitute.For<StorableDetectorDataSet>();
		var tag = Substitute.For<StorableTag>();
		set.TagsLibrary.Tags.Returns([tag]);
		var image = Substitute.For<StorableImage>();
		var asset = Substitute.For<StorableItemsAsset<StorableDetectorItem>>();
		set.AssetsLibrary.Assets.Returns([asset]);
		var item = Substitute.For<StorableDetectorItem>();
		asset.Items.Returns([item]);
		item.Tag.Returns(tag);
		asset.Image.Returns(image);
		tag.Users.Returns([item]);
		var persistedSet = set.PersistUsingFormatter(Formatter);
		var persistedTag = persistedSet.TagsLibrary.Tags.Single();
		var persistedAsset = persistedSet.AssetsLibrary.Assets.Single();
		var persistedItem = persistedAsset.Items.Single();
		persistedTag.Users.Should().Contain(persistedItem);
	}

	private static DetectorDataSetFormatter Formatter => new(Substitute.For<ImageLookupper>(),
		new WrappingDetectorDataSetFactory(new DetectorDataSetWrapper(Substitute.For<ChangeListener>(), new Lock()), Substitute.For<ChangeListener>(), new Lock()));
}