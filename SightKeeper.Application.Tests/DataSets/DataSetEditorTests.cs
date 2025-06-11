using FluentAssertions;
using FluentValidation;
using NSubstitute;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.Tests.DataSets.Fakes;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Application.Tests.DataSets;

public sealed class DataSetEditorTests
{
	[Fact]
	public void ShouldChangeDataSetProperties()
	{
		const string name = "new name";
		const string description = "new description";
		var dataSet = CreateDataSet();
		var editor = CreateEditor();
		FakeExistingDataSetData data = new(dataSet, name, description);
		editor.Edit(data);
		dataSet.Name.Should().Be(name);
		dataSet.Description.Should().Be(description);
	}

	[Fact]
	public void ShouldNotifyObserver()
	{
		var dataSet = CreateDataSet();
		var editor = CreateEditor();
		FakeExistingDataSetData data = new(dataSet, "new name", "new description");
		var observer = Substitute.For<IObserver<DataSet>>();
		editor.DataSetEdited.Subscribe(observer);
		editor.Edit(data);
		observer.Received().OnNext(dataSet);
	}

	[Fact]
	public void ShouldNotChangeDataSetPropertiesWhenValidationFails()
	{
		const string initialName = "the name";
		const string initialDescription = "the description";
		var dataSet = CreateDataSet(initialName, initialDescription);
		var editor = CreateEditorWithImpassableValidator();
		FakeExistingDataSetData data = new(dataSet, "new name", "new description");
		Assert.Throws<ValidationException>(() => editor.Edit(data));
		dataSet.Name.Should().Be(initialName);
		dataSet.Description.Should().Be(initialDescription);
	}

	[Fact]
	public void ShouldNotNotifyObserverWhenValidationFails()
	{
		var dataSet = CreateDataSet();
		var editor = CreateEditorWithImpassableValidator();
		FakeExistingDataSetData data = new(dataSet, "new name", "new description");
		var observer = Substitute.For<IObserver<DataSet>>();
		editor.DataSetEdited.Subscribe(observer);
		Assert.Throws<ValidationException>(() => editor.Edit(data));
		observer.DidNotReceive().OnNext(dataSet);
	}

	[Fact]
	public void ShouldRemoveTag()
	{
		var dataSet = CreateDataSet();
		var tag = dataSet.TagsLibrary.CreateTag(string.Empty);
		var editor = CreateEditor();
		var data = FakeExistingDataSetData.CreateWithRemovedTags(dataSet, tag);
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
		var data = FakeExistingDataSetData.CreateWithEditedTag(dataSet, tag, newName);
		editor.Edit(data);
		tag.Name.Should().Be(newName);
	}

	[Fact]
	public void ShouldCreateTag()
	{
		var dataSet = CreateDataSet();
		var editor = CreateEditor();
		const string newName = "new name";
		var data = FakeExistingDataSetData.CreateWithNewTags(dataSet, newName);
		editor.Edit(data);
		dataSet.TagsLibrary.Tags.Should().Contain(tag => tag.Name == newName);
	}

	[Fact]
	public void ShouldCreatePoserTagWithKeyPointTags()
	{
		var dataSet = CreatePoserDataSet();
		var editor = CreateEditor();
		const string keyPointTagName = "key point tag";
		var data = FakeExistingDataSetData.CreateWithNewPoserTag(dataSet, "new poser tag", keyPointTagName);
		editor.Edit(data);
		dataSet.TagsLibrary.Tags.Should().Contain(tag => tag.KeyPointTags.Any(keyPointTag => keyPointTag.Name == keyPointTagName));
	}

	[Fact]
	public void ShouldRemoveKeyPointTag()
	{
		var dataSet = CreatePoserDataSet();
		var poserTag = dataSet.TagsLibrary.CreateTag(string.Empty);
		var keyPointTag = poserTag.CreateKeyPointTag(string.Empty);
		var editor = CreateEditor();
		var data = FakeExistingDataSetData.CreateWithRemovedKeyPointTag(dataSet, poserTag, keyPointTag);
		editor.Edit(data);
		poserTag.KeyPointTags.Should().BeEmpty();
	}

	private static DataSet CreateDataSet(string name = "", string description = "")
	{
		return new DomainClassifierDataSet
		{
			Name = name,
			Description = description
		};
	}

	private static DataSetEditor CreateEditor()
	{
		return new DataSetEditor
		{
			Validator = CreateValidator()
		};
	}

	private static IValidator<ExistingDataSetData> CreateValidator()
	{
		return new InlineValidator<ExistingDataSetData>();
	}

	private static DataSetEditor CreateEditorWithImpassableValidator()
	{
		return new DataSetEditor
		{
			Validator = CreateImpassableValidator()
		};
	}

	private static IValidator<ExistingDataSetData> CreateImpassableValidator()
	{
		InlineValidator<ExistingDataSetData> validator = new();
		validator.RuleFor(data => data.Name).Must(_ => false);
		return validator;
	}

	private static PoserDataSet CreatePoserDataSet()
	{
		return new DomainPoser2DDataSet();
	}
}