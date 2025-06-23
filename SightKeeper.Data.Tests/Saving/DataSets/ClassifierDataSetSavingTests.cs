using FlakeId;
using FluentAssertions;
using NSubstitute;
using SightKeeper.Data.DataSets.Classifier;
using SightKeeper.Data.Images;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Tests.Saving.DataSets;

public sealed class ClassifierDataSetSavingTests
{
	[Fact]
	public void ShouldPersistName()
	{
		const string name = "The name";
		var set = Substitute.For<ClassifierDataSet>();
		set.Name.Returns(name);
		var persistedSet = set.PersistUsingFormatter(Formatter);
		persistedSet.Name.Should().Be(name);
	}

	[Fact]
	public void ShouldPersistDescription()
	{
		const string description = "The description";
		var set = Substitute.For<ClassifierDataSet>();
		set.Description.Returns(description);
		var persistedSet = set.PersistUsingFormatter(Formatter);
		persistedSet.Description.Should().Be(description);
	}

	[Fact]
	public void ShouldPersistTagName()
	{
		const string tagName = "The tag";
		var set = Substitute.For<ClassifierDataSet>();
		var tag = Substitute.For<Tag>();
		tag.Name.Returns(tagName);
		set.TagsLibrary.Tags.Returns([tag]);
		var persistedSet = set.PersistUsingFormatter(Formatter);
		persistedSet.TagsLibrary.Tags.Should().ContainSingle()
			.Which.Name.Should().Be(tagName);
	}

	[Fact]
	public void ShouldPersistTagColor()
	{
		const uint tagColor = 1234;
		var set = Substitute.For<ClassifierDataSet>();
		var tag = Substitute.For<Tag>();
		set.TagsLibrary.Tags.Returns([tag]);
		tag.Color.Returns(tagColor);
		var persistedSet = set.PersistUsingFormatter(Formatter);
		persistedSet.TagsLibrary.Tags.Should().ContainSingle()
			.Which.Color.Should().Be(tagColor);
	}

	[Fact]
	public void ShouldPersistAsset()
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

	private static ClassifierDataSetFormatter Formatter => new(Substitute.For<ImageLookupper>());
}