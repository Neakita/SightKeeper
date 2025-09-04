using FlakeId;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Detector;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.DataSets.Detector;

public sealed class DetectorDataSetFormatterTests
{
	[Fact]
	public void ShouldPersistItemTag()
	{
		var set = Substitute.For<DetectorDataSet>();
		var tag = Substitute.For<Tag>();
		set.TagsLibrary.Tags.Returns([tag]);
		var image = new InMemoryImage(Id.Create(), default, default);
		var asset = Substitute.For<ItemsAsset<DetectorItem>>();
		var item = Substitute.For<DetectorItem>();
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
		var set = Substitute.For<DetectorDataSet>();
		var tag = Substitute.For<Tag>();
		set.TagsLibrary.Tags.Returns([tag]);
		var image = Substitute.For<ManagedImage>();
		var asset = Substitute.For<ItemsAsset<DetectorItem>>();
		set.AssetsLibrary.Assets.Returns([asset]);
		var item = Substitute.For<DetectorItem>();
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