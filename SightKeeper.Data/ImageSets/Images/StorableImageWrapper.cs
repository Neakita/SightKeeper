using SightKeeper.Data.Services;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class StorableImageWrapper(FileSystemDataAccess dataAccess) : ImageWrapper
{
	public StorableImage Wrap(StorableImage image)
	{
		return image
			.WithStreaming(dataAccess)
			.WithObservableAssets();
	}
}