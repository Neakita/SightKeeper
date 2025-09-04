using FlakeId;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.DataSets.Classifier;

public sealed class ClassifierDataSetSavingTests
{
	[Fact]
	public void ShouldPersistAssetTag()
	{
		var set = Substitute.For<ClassifierDataSet>();
		var tag = Substitute.For<Tag>();
		set.TagsLibrary.Tags.Returns([tag]);
		var image = new InMemoryImage(Id.Create(), default, default);
		var asset = Substitute.For<ClassifierAsset>();
		set.AssetsLibrary.Assets.Returns([asset]);
		asset.Tag.Returns(tag);
		asset.Image.Returns(image);
		var persistedSet = set.PersistUsingFormatter(Formatter);
		var persistedTag = persistedSet.TagsLibrary.Tags.Single();
		persistedSet.AssetsLibrary.Assets.Should().ContainSingle().Which.Tag.Should().Be(persistedTag);
	}

	[Fact]
	public void ShouldPersistTagUser()
	{
		var set = Substitute.For<ClassifierDataSet>();
		var tag = Substitute.For<Tag>();
		set.TagsLibrary.Tags.Returns([tag]);
		var image = Substitute.For<ManagedImage>();
		var asset = Substitute.For<ClassifierAsset>();
		set.AssetsLibrary.Assets.Returns([asset]);
		asset.Tag.Returns(tag);
		asset.Image.Returns(image);
		tag.Users.Returns([asset]);
		var persistedSet = set.PersistUsingFormatter(Formatter);
		var persistedTag = persistedSet.TagsLibrary.Tags.Single();
		var persistedAsset = persistedSet.AssetsLibrary.Assets.Single();
		persistedTag.Users.Should().Contain(persistedAsset);
	}

	private static ClassifierDataSetFormatter Formatter => new(Substitute.For<ImageLookupper>(),
		new WrappingClassifierDataSetFactory(new ClassifierDataSetWrapper(Substitute.For<ChangeListener>(), new Lock()), Substitute.For<ChangeListener>(), new Lock()));
}