using FluentAssertions;
using NSubstitute;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests;

public sealed class ExistingImageSetDataValidatorTests
{
	[Fact]
	public void ShouldPassValidation()
	{
		ImageSet set1 = new() { Name = "the set 1" };
		ImageSet set2 = new() { Name = "the set 2" };
		ImageSet set3 = new() { Name = "the set 3" };
		var validator = CreateValidator(set1, set2, set3);
		var data = Utilities.CreateImageSetData("the set 4");
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
		var data = Utilities.CreateImageSetData("the set 2");
		var result = validator.Validate(data);
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(failure => failure.PropertyName == nameof(ImageSetData.Name));
	}

	private static ExistingImageSetDataValidator CreateValidator(ImageSet set, params IEnumerable<ImageSet> otherSets)
	{
		var repository = CreateRepository(otherSets.Prepend(set));
		ExistingImageSetDataValidator validator = new(set, repository);
		return validator;
	}

	private static ReadRepository<ImageSet> CreateRepository(IEnumerable<ImageSet> sets)
	{
		var repository = Substitute.For<ReadRepository<ImageSet>>();
		repository.Items.Returns(sets.ToList());
		return repository;
	}
}