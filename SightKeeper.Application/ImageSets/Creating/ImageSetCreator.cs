using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Creating;

public sealed class ImageSetCreator
{
	public ImageSetCreator(WriteRepository<ImageSet> repository)
	{
		_repository = repository;
	}

	public ImageSet Create(ImageSetData data)
	{
		ImageSet library = new()
		{
			Name = data.Name,
			Description = data.Description
		};
		_repository.Add(library);
		return library;
	}

	private readonly WriteRepository<ImageSet> _repository;
}