using FluentAssertions;
using FluentValidation;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Application.Tests.DataSets;

public sealed class NewDataSetDataValidatorTests
{
	[Fact]
	public void ShouldPassValidation()
	{
		var validator = CreateValidator("data set 1", "data set 2");
		var data = Utilities.CreateNewDataSetData("data set 3");
		var result = validator.Validate(data);
		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void ShouldNotPassValidationWhenNameCollides()
	{
		var validator = CreateValidator("data set 1", "data set 2");
		var data = Utilities.CreateNewDataSetData("data set 2");
		var result = validator.Validate(data);
		result.Errors.Should().Contain(failure => failure.PropertyName == nameof(NewDataSetData.Name));
	}

	private static IValidator<NewDataSetData> CreateValidator(params IEnumerable<string> existingDataSetsNames)
	{
		var existingDataSets = existingDataSetsNames.Select(name => new ClassifierDataSet { Name = name }).ToList();
		var repository = Utilities.CreateRepository(existingDataSets);
		return new NewDataSetDataValidator(repository);
	}
}