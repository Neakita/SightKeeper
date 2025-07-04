using FluentAssertions;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests.ImageSets;

public sealed class ExistingImageSetDataValidatorTests
{
	[Fact]
	public void ShouldPassValidation()
	{
		ImageSet set1 = new() { Name = "the set 1" };
		ImageSet set2 = new() { Name = "the set 2" };
		ImageSet set3 = new() { Name = "the set 3" };
		var validator = CreateValidator(set1, set2, set3);
		var data = Utilities.CreateExistingImageSetData(set1, "the set 4");
		var result = validator.Validate(data);
		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void ShouldNotPassValidationWhenNameIsOccupiedByAnotherSet()
	{
		ImageSet set1 = new() { Name = "the set 1" };
		ImageSet set2 = new() { Name = "the set 2" };
		ImageSet set3 = new() { Name = "the set 3" };
		var validator = CreateValidator(set1, set2, set3);
		var data = Utilities.CreateExistingImageSetData(set1, "the set 2");
		var result = validator.Validate(data);
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(failure => failure.PropertyName == nameof(ImageSetData.Name));
	}

	private static ExistingImageSetDataValidator CreateValidator(params IReadOnlyCollection<ImageSet> sets)
	{
		var repository = Utilities.CreateRepository(sets);
		ExistingImageSetDataValidator validator = new(repository);
		return validator;
	}
}