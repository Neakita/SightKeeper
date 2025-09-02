using SightKeeper.Data.Services;

namespace SightKeeper.Data.ImageSets.Images;

internal static class ImageExtensions
{
    public static StorableImage WithStreaming(this StorableImage image, FileSystemDataAccess dataAccess)
    {
        return new StreamableDataImage(image, dataAccess);
    }

    public static StorableImage WithObservableAssets(this StorableImage image)
    {
        return new ObservableAssetsImage(image);
    }
}