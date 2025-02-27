using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Creating;

public sealed class ImageSetCreator
{
	public ImageSetCreator(WriteDataAccess<ImageSet> dataAccess)
	{
		_dataAccess = dataAccess;
	}

	public ImageSet Create(ImageSetData data)
	{
		ImageSet library = new()
		{
			Name = data.Name,
			Description = data.Description
		};
		_dataAccess.Add(library);
		return library;
	}

	private readonly WriteDataAccess<ImageSet> _dataAccess;
}