using FlakeId;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data.Tests.DataSets;

public sealed class ClassifierDataSetSavingTests
{
	[Fact]
	public void ShouldPersistAssetTag()
	{
		var set = Substitute.For<StorableClassifierDataSet>();
		var tag = Substitute.For<StorableTag>();
		set.TagsLibrary.Tags.Returns([tag]);
		var image = new InMemoryImage(Id.Create(), default, default);
		var asset = Substitute.For<StorableClassifierAsset>();
		set.AssetsLibrary.Assets.Returns([asset]);
		asset.Tag.Returns(tag);
		asset.Image.Returns(image);
		var persistedSet = set.PersistUsingFormatter(Formatter);
		var persistedTag = persistedSet.TagsLibrary.Tags.Single();
		persistedSet.AssetsLibrary.Assets.Should().ContainSingle().Which.Tag.Should().Be(persistedTag);
	}

	private static ClassifierDataSetFormatter Formatter => new()
	{
		ImageLookupper = Substitute.For<ImageLookupper>(),
		SetWrapper = new ClassifierDataSetWrapper(Substitute.For<ChangeListener>(), new Lock())
	};
}