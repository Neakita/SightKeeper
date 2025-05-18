using FluentValidation;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Creating;

public sealed class ImageSetCreator
{
	[Tag("new")] public required IValidator<ImageSetData> Validator { get; init; }
	public required WriteRepository<ImageSet> Repository { get; init; }

	public ImageSet Create(ImageSetData data)
	{
		Validator.ValidateAndThrow(data);
		ImageSet library = new()
		{
			Name = data.Name,
			Description = data.Description
		};
		Repository.Add(library);
		return library;
	}
}