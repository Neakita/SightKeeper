using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class StorableImageWrapper(FileSystemDataAccess dataAccess) : ImageWrapper
{
	public Image Wrap(InMemoryImage image)
	{
		return image
			.WithStreaming(dataAccess)
			.WithObservableAssets();
	}
}