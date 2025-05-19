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
		var dataSet = CreateDataSet();
		DataSetEditor editor = new()
		{
			Validator = CreateValidator()
		};
		const string name = "new name";
		const string description = "new description";
		var data = Utilities.CreateExistingDataSetData(dataSet, name, description);
		editor.Edit(data);
		dataSet.Name.Should().Be(name);
		dataSet.Description.Should().Be(description);
	}

	[Fact]
	public void ShouldNotifyObserver()
	{
		var dataSet = CreateDataSet();
		DataSetEditor editor = new()
		{
			Validator = CreateValidator()
		};
		var data = Utilities.CreateExistingDataSetData(dataSet, "new name", "new description");
		var observer = Substitute.For<IObserver<DataSet>>();
		editor.DataSetEdited.Subscribe(observer);
		editor.Edit(data);
		observer.Received().OnNext(dataSet);
	}

	[Fact]
	public void ShouldNotChangeDataSetPropertiesWhenValidationFails()
	{
		const string name = "old name";
		const string description = "old description";
		var dataSet = CreateDataSet(name, description);
		DataSetEditor editor = new()
		{
			Validator = CreateImpassableValidator()
		};
		var data = Utilities.CreateExistingDataSetData(dataSet, "new name", "new description");
		Assert.Throws<ValidationException>(() => editor.Edit(data));
		dataSet.Name.Should().Be(name);
		dataSet.Description.Should().Be(description);
	}

	[Fact]
	public void ShouldNotNotifyObserverWhenValidationFails()
	{
		var dataSet = CreateDataSet();
		DataSetEditor editor = new()
		{
			Validator = CreateImpassableValidator()
		};
		var data = Utilities.CreateExistingDataSetData(dataSet, "new name", "new description");
		var observer = Substitute.For<IObserver<DataSet>>();
		editor.DataSetEdited.Subscribe(observer);
		Assert.Throws<ValidationException>(() => editor.Edit(data));
		observer.DidNotReceive().OnNext(dataSet);
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