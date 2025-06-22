/*using FluentAssertions;
using FluentValidation;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.Tests.DataSets.Fakes;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Application.Tests.DataSets;

public sealed class ExistingDataSetDataValidatorTests
{
	[Fact]
	public void ShouldPassValidation()
	{
		var dataSet1 = CreateDataSet("data set 1");
		var dataSet2 = CreateDataSet("data set 2");
		var validator = CreateValidator(dataSet1, dataSet2);
		var data = new FakeExistingDataSetData(dataSet2, "data set 3");
		var result = validator.Validate(data);
		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void ShouldNotPassValidationWhenNameCollidesWithOtherDataSet()
	{
		var dataSet1 = CreateDataSet("data set 1");
		var dataSet2 = CreateDataSet("data set 2");
		var validator = CreateValidator(dataSet1, dataSet2);
		var data = new FakeExistingDataSetData(dataSet1, "data set 2");
		var result = validator.Validate(data);
		result.Errors.Should().Contain(failure => failure.PropertyName == nameof(ExistingDataSetData.Name));
	}

	private static DataSet CreateDataSet(string name)
	{
		return new DomainClassifierDataSet
		{
			Name = name
		};
	}

	private static IValidator<ExistingDataSetData> CreateValidator(params IReadOnlyCollection<DataSet> dataSets)
	{
		var repository = Utilities.CreateRepository(dataSets);
		return new ExistingDataSetDataValidator(repository);
	}
}*/