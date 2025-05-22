using FluentAssertions;
using FluentValidation;
using NSubstitute;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Application.Tests.DataSets;

public sealed class DataSetEditorPoserTagsTests
{
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

	[Fact]
	public void ShouldRemoveKeyPointTag()
	{
		var dataSet = CreatePoserDataSet();
		var poserTag = dataSet.TagsLibrary.CreateTag(string.Empty);
		var keyPointTag = poserTag.CreateKeyPointTag(string.Empty);
		DataSetEditor editor = new()
		{
			Validator = CreateValidator()
		};
		var data = Substitute.For<ExistingDataSetData>();
		data.DataSet.Returns(dataSet);
		var tagsChanges = Substitute.For<TagsChanges>();
		data.TagsChanges.Returns(tagsChanges);
		var editedPoserTag = Substitute.For<EditedPoserTagData>();
		editedPoserTag.Tag.Returns(poserTag);
		tagsChanges.EditedTags.Returns([editedPoserTag]);
		editedPoserTag.KeyPointTagsChanges.RemovedTags.Returns([keyPointTag]);
		editor.Edit(data);
		poserTag.KeyPointTags.Should().BeEmpty();
	}

	private static PoserDataSet CreatePoserDataSet()
	{
		return new Poser2DDataSet();
	}

	private static IValidator<ExistingDataSetData> CreateValidator()
	{
		return new InlineValidator<ExistingDataSetData>();
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