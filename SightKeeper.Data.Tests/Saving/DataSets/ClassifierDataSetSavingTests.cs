using FluentAssertions;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.Tests.Saving.DataSets;

public sealed class ClassifierDataSetSavingTests
{
	[Fact]
	public void ShouldPersistName()
	{
		const string name = "The name";
		var set = Utilities.CreateClassifierDataSet();
		set.Name = name;
		var persistedSet = (ClassifierDataSet)set.Persist<DataSet>();
		persistedSet.Name.Should().Be(name);
	}

	[Fact]
	public void ShouldPersistDescription()
	{
		const string description = "The description";
		var set = Utilities.CreateClassifierDataSet();
		set.Description = description;
		var persistedSet = (ClassifierDataSet)set.Persist<DataSet>();
		persistedSet.Description.Should().Be(description);
	}

	[Fact]
	public void ShouldPersistTagName()
	{
		const string tagName = "The tag";
		var set = Utilities.CreateClassifierDataSet();
		set.TagsLibrary.CreateTag(tagName);
		var persistedSet = (ClassifierDataSet)set.Persist<DataSet>();
		persistedSet.TagsLibrary.Tags.Should().ContainSingle()
			.Which.Name.Should().Be(tagName);
	}

	[Fact]
	public void ShouldPersistTagColor()
	{
		const uint tagColor = 1234;
		var set = Utilities.CreateClassifierDataSet();
		var tag = set.TagsLibrary.CreateTag("");
		tag.Color = tagColor;
		var persistedSet = (ClassifierDataSet)set.Persist<DataSet>();
		persistedSet.TagsLibrary.Tags.Should().ContainSingle()
			.Which.Color.Should().Be(tagColor);
	}

	[Fact]
	public void ShouldPersistAsset()
	{
		var set = Utilities.CreateClassifierDataSet();
		set.TagsLibrary.CreateTag("");
		var image = Utilities.CreateImage();
		set.AssetsLibrary.MakeAsset(image);
		var persistedSet = (ClassifierDataSet)set.Persist<DataSet>();
		var tag = persistedSet.TagsLibrary.Tags.Single();
		persistedSet.AssetsLibrary.Assets.Should().ContainSingle().Which.Tag.Should().Be(tag);
	}
}