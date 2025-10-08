using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal sealed class StorableImageWrapper(FileSystemDataAccess dataAccess) : ImageWrapper
{
	public ManagedImage Wrap(ManagedImage image)
	{
		return image
			.WithStreaming(dataAccess)
			.WithObservableAssets();
	}
}