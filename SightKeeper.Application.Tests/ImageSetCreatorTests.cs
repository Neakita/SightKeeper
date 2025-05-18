using FluentAssertions;
using FluentValidation;
using NSubstitute;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests;

public sealed class ImageSetCreatorTests
{
	[Fact]
	public void ShouldAddNewImageSetToRepository()
	{
		var repository = Substitute.For<WriteRepository<ImageSet>>();
		ImageSetCreator imageSetCreator = new()
		{
			Validator = new ImageSetDataValidator(),
			Repository = repository
		};
		const string name = "name";
		const string description = "description";
		var data = Utilities.CreateImageSetData(name, description);
		var imageSet = imageSetCreator.Create(data);
		repository.Received().Add(imageSet);
		imageSet.Name.Should().Be(name);
		imageSet.Description.Should().Be(description);
	}

	[Fact]
	public void ShouldThrowWhenValidationFails()
	{
		var repository = Substitute.For<WriteRepository<ImageSet>>();
		ImageSetCreator imageSetCreator = new()
		{
			Validator = new ImageSetDataValidator(),
			Repository = repository
		};
		const string name = "";
		const string description = "description";
		var data = Utilities.CreateImageSetData(name, description);
		Assert.Throws<ValidationException>(() => imageSetCreator.Create(data));
		repository.DidNotReceive().Add(Arg.Any<ImageSet>());
	}
}