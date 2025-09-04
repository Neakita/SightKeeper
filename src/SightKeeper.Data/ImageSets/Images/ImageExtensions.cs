using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal static class ImageExtensions
{
    public static ManagedImage WithStreaming(this ManagedImage image, FileSystemDataAccess dataAccess)
    {
        return new StreamableDataImage(image, dataAccess);
    }

    public static ManagedImage WithObservableAssets(this ManagedImage image)
    {
        return new ObservableAssetsImage(image);
    }
}