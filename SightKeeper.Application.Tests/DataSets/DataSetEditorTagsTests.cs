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

	/*private static EditedPoserTagData CreateEditedPoserTagDataWithRemovedKeyPointTag()
	{
		var data = Substitute.For<EditedPoserTagData>();
		data.
	}*/

	private static ExistingDataSetData CreateDataWithEditedPoserTag(PoserDataSet dataSet, params IEnumerable<EditedPoserTagData> editedTags)
	{
		var data = Utilities.CreateExistingDataSetData(dataSet);
		var tagsChanges = Substitute.For<TagsChanges>();
		tagsChanges.EditedTags.Returns(editedTags);
		data.TagsChanges.Returns(tagsChanges);
		return data;
	}
}