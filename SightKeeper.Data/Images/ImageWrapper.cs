using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Images;

internal sealed class ImageWrapper(FileSystemDataAccess dataAccess)
{
	public Image Wrap(InMemoryImage image)
	{
		return image
			.WithStreaming(dataAccess)
			.WithObservableAssets();
	}
}