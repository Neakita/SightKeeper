using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using SightKeeper.Application.DataSets;

namespace SightKeeper.Application.Tests.DataSets;

public sealed class DataSetDataValidatorTests
{
	[Fact]
	public void ShouldPassValidation()
	{
		var result = Validate("the name", "the description");
		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void ShouldNotPassValidationWithEmptyName()
	{
		var result = Validate(string.Empty, "the description");
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(failure => failure.PropertyName == nameof(DataSetData.Name));
	}

	[Fact]
	public void ShouldPassValidationWithEmptyDescription()
	{
		var result = Validate("the name", string.Empty);
		result.IsValid.Should().BeTrue();
	}

	private static ValidationResult Validate(string name, string description)
	{
		var validator = CreateValidator();
		var data = CreateData(name, description);
		return validator.Validate(data);	
	}

	private static IValidator<DataSetData> CreateValidator()
	{
		return DataSetDataValidator.Instance;
	}

	private static DataSetData CreateData(string name, string description)
	{
		var data = Substitute.For<DataSetData>();
		data.Name.Returns(name);
		data.Description.Returns(description);
		return data;
	}
}