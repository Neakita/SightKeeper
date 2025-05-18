using FluentAssertions;
using SightKeeper.Application.ImageSets;

namespace SightKeeper.Application.Tests;

public sealed class ImageSetDataValidatorTests
{
	[Fact]
	public void ShouldPassValidation()
	{
		ImageSetDataValidator validator = new();
		var data = Utilities.CreateImageSetData("the name", "the description");
		var result = validator.Validate(data);
		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void ShouldNotPassValidationWithEmptyName()
	{
		ImageSetDataValidator validator = new();
		var data = Utilities.CreateImageSetData(string.Empty, "the description");
		var result = validator.Validate(data);
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(failure => failure.PropertyName == nameof(ImageSetData.Name));
	}

	[Fact]
	public void ShouldPassValidationWithEmptyDescription()
	{
		ImageSetDataValidator validator = new();
		var data = Utilities.CreateImageSetData("the name", string.Empty);
		var result = validator.Validate(data);
		result.IsValid.Should().BeTrue();
	}
}