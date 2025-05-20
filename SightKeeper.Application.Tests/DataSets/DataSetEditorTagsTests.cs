using FluentAssertions;
using FluentValidation;
using NSubstitute;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Tests.DataSets;

public sealed class DataSetEditorTagsTests
{
	[Fact]
	public void ShouldRemoveTag()
	{
		var dataSet = CreateDataSet();
		var tag = dataSet.TagsLibrary.CreateTag(string.Empty);
		DataSetEditor editor = new()
		{
			Validator = CreateValidator()
		};
		var data = CreateDataWithRemovedTags(dataSet, tag);
		editor.Edit(data);
		dataSet.TagsLibrary.Tags.Should().BeEmpty();
	}

	[Fact]
	public void ShouldEditTagName()
	{
		var dataSet = CreateDataSet();
		var tag = dataSet.TagsLibrary.CreateTag(string.Empty);
		DataSetEditor editor = new()
		{
			Validator = CreateValidator()
		};
		const string newName = "new name";
		var data = CreateDataWithEditedTag(dataSet, tag, newName);
		editor.Edit(data);
		tag.Name.Should().Be(newName);
	}

	[Fact]
	public void ShouldCreateTag()
	{
		var dataSet = CreateDataSet();
		DataSetEditor editor = new()
		{
			Validator = CreateValidator()
		};
		const string newName = "new name";
		var data = CreateDataWithNewTag(dataSet, newName);
		editor.Edit(data);
		dataSet.TagsLibrary.Tags.Should().Contain(tag => tag.Name == newName);
	}

	[Fact]
	public void ShouldCreatePoserTagWithKeyPointTags()
	{
		var dataSet = CreatePoserDataSet();
		DataSetEditor editor = new()
		{
			Validator = CreateValidator()
		};
		const string newName = "new name";
		const string keyPointTagName = "key point tag";
		var data = CreateDataWithNewPoserTag(dataSet, newName, keyPointTagName);
		editor.Edit(data);
		dataSet.TagsLibrary.Tags.Should().Contain(tag => tag.KeyPointTags.Any(keyPointTag => keyPointTag.Name == keyPointTagName));
	}

	private static DataSet CreateDataSet()
	{
		return new ClassifierDataSet();
	}

	private static IValidator<ExistingDataSetData> CreateValidator()
	{
		return new InlineValidator<ExistingDataSetData>();
	}

	private static ExistingDataSetData CreateDataWithRemovedTags(DataSet dataSet, params IEnumerable<Tag> removedTags)
	{
		var data = Utilities.CreateExistingDataSetData(dataSet);
		var tagsChanges = Substitute.For<TagsChanges>();
		tagsChanges.RemovedTags.Returns(removedTags);
		data.TagsChanges.Returns(tagsChanges);
		return data;
	}

	private static ExistingDataSetData CreateDataWithEditedTag(DataSet dataSet, Tag tag, string newName)
	{
		var data = Utilities.CreateExistingDataSetData(dataSet);
		var tagsChanges = Substitute.For<TagsChanges>();
		var editedTagData = Substitute.For<EditedTagData>();
		editedTagData.Tag.Returns(tag);
		editedTagData.Name.Returns(newName);
		tagsChanges.EditedTags.Returns([editedTagData]);
		data.TagsChanges.Returns(tagsChanges);
		return data;
	}

	private static ExistingDataSetData CreateDataWithNewTag(DataSet dataSet, string newTagName)
	{
		var data = Utilities.CreateExistingDataSetData(dataSet);
		var tagsChanges = Substitute.For<TagsChanges>();
		var newTagData = Substitute.For<NewTagData>();
		newTagData.Name.Returns(newTagName);
		tagsChanges.NewTags.Returns([newTagData]);
		data.TagsChanges.Returns(tagsChanges);
		return data;
	}

	private static PoserDataSet CreatePoserDataSet()
	{
		return new Poser2DDataSet();
	}

	private static ExistingDataSetData CreateDataWithNewPoserTag(PoserDataSet dataSet, string newPoserTagName, params IEnumerable<string> keyPointTagNames)
	{
		var data = Utilities.CreateExistingDataSetData(dataSet);
		var tagsChanges = Substitute.For<TagsChanges>();
		var newTagData = Substitute.For<NewPoserTagData>();
		newTagData.Name.Returns(newPoserTagName);
		var keyPointTags = keyPointTagNames.Select(name =>
		{
			var keyPointTagData = Substitute.For<NewTagData>();
			keyPointTagData.Name.Returns(name);
			return keyPointTagData;
		});
		newTagData.KeyPointTags.Returns(keyPointTags);
		tagsChanges.NewTags.Returns([newTagData]);
		data.TagsChanges.Returns(tagsChanges);
		return data;
	}
}