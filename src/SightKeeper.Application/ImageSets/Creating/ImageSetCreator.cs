using FluentValidation;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Creating;

public sealed class ImageSetCreator
{
	public required IValidator<ImageSetData> Validator { get; init; }
	public required Factory<ImageSet> Factory { get; init; }
	public required WriteRepository<ImageSet> Repository { get; init; }

	public ImageSet Create(ImageSetData data)
	{
		Validator.ValidateAndThrow(data);
		var set = Factory.Create();
		set.Name = data.Name;
		set.Description = data.Description;
		Repository.Add(set);
		return set;
	}
}