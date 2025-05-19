using FluentAssertions;
using FluentValidation;
using NSubstitute;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Application.Tests.DataSets;

public sealed class DataSetEditorTests
{
	[Fact]
	public void ShouldChangeDataSetProperties()
	{
		const string name = "new name";
		const string description = "new description";
		EditDataSetViaEditor(name, description, out var dataSet);
		dataSet.Name.Should().Be(name);
		dataSet.Description.Should().Be(description);
	}

	[Fact]
	public void ShouldNotifyObserver()
	{
		EditDataSetViaEditor(out var dataSet, out var observer);
		observer.Received().OnNext(dataSet);
	}

	[Fact]
	public void ShouldNotChangeDataSetPropertiesWhenValidationFails()
	{
		const string initialName = "the name";
		const string initialDescription = "the description";
		AttemptToEditDataSetWithValidationExceptionProvocation(initialName, initialDescription, out var dataSet);
		dataSet.Name.Should().Be(initialName);
		dataSet.Description.Should().Be(initialDescription);
	}

	[Fact]
	public void ShouldNotNotifyObserverWhenValidationFails()
	{
		AttemptToEditDataSetWithValidationExceptionProvocation(out var dataSet, out var observer);
		observer.DidNotReceive().OnNext(dataSet);
	}

	private static void EditDataSetViaEditor(string name, string description, out DataSet dataSet)
	{
		dataSet = CreateDataSet();
		DataSetEditor editor = new()
		{
			Validator = CreateValidator()
		};
		var data = Utilities.CreateExistingDataSetData(dataSet, name, description);
		editor.Edit(data);
	}

	private static void EditDataSetViaEditor(out DataSet dataSet, out IObserver<DataSet> observer)
	{
		dataSet = CreateDataSet();
		DataSetEditor editor = new()
		{
			Validator = CreateValidator()
		};
		var data = Utilities.CreateExistingDataSetData(dataSet, "new name", "new description");
		observer = Substitute.For<IObserver<DataSet>>();
		editor.DataSetEdited.Subscribe(observer);
		editor.Edit(data);
	}

	private static void AttemptToEditDataSetWithValidationExceptionProvocation(string initialName, string initialDescription, out DataSet dataSet)
	{
		dataSet = CreateDataSet(initialName, initialDescription);
		DataSetEditor editor = new()
		{
			Validator = CreateImpassableValidator()
		};
		var data = Utilities.CreateExistingDataSetData(dataSet, "new name", "new description");
		Assert.Throws<ValidationException>(() => editor.Edit(data));
	}

	private static void AttemptToEditDataSetWithValidationExceptionProvocation(out DataSet dataSet, out IObserver<DataSet> observer)
	{
		dataSet = CreateDataSet();
		DataSetEditor editor = new()
		{
			Validator = CreateImpassableValidator()
		};
		var data = Utilities.CreateExistingDataSetData(dataSet, "new name", "new description");
		observer = Substitute.For<IObserver<DataSet>>();
		editor.DataSetEdited.Subscribe(observer);
		Assert.Throws<ValidationException>(() => editor.Edit(data));
	}

	private static DataSet CreateDataSet(string name = "", string description = "")
	{
		return new ClassifierDataSet
		{
			Name = name,
			Description = description
		};
	}

	private static IValidator<ExistingDataSetData> CreateValidator()
	{
		return new InlineValidator<ExistingDataSetData>();
	}

	private static IValidator<ExistingDataSetData> CreateImpassableValidator()
	{
		InlineValidator<ExistingDataSetData> validator = new();
		validator.RuleFor(data => data.Name).Must(_ => false);
		return validator;
	}
}