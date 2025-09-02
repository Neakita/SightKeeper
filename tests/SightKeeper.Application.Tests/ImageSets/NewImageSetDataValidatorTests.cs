/*using FluentAssertions;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests.ImageSets;

public sealed class NewImageSetDataValidatorTests
{
	[Fact]
	public void ShouldPassValidation()
	{
		var validator = CreateValidator("set 1", "set 2");
		var data = Utilities.CreateImageSetData("set 3");
		var result = validator.Validate(data);
		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void ShouldNotPassValidationWhenRepositoryContainsSetWithSameName()
	{
		var validator = CreateValidator("set 1", "set 2");
		var data = Utilities.CreateImageSetData("set 2");
		var result = validator.Validate(data);
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(failure => failure.PropertyName == nameof(ImageSetData.Name));
	}

	private static NewImageSetDataValidator CreateValidator(params IEnumerable<string> repositorySetsNames)
	{
		var sets = repositorySetsNames.Select(name => new DomainImageSet { Name = name }).ToList();
		return CreateValidator(sets);
	}

	private static NewImageSetDataValidator CreateValidator(params IReadOnlyCollection<DomainImageSet> repositorySets)
	{
		var imageSetsRepository = Utilities.CreateRepository(repositorySets);
		NewImageSetDataValidator validator = new(imageSetsRepository);
		return validator;
	}
}*/