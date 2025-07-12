using FlakeId;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal static class ImageExtensions
{
    public static Image WithStreaming(this InMemoryImage image, FileSystemDataAccess dataAccess)
    {
        return new StreamableDataImage(image, dataAccess);
    }

    public static Image WithObservableAssets(this Image image)
    {
        return new ObservableAssetsImage(image);
    }

    public static Id GetId(this Image image)
    {
        var inMemoryImage = image.UnWrapDecorator<InMemoryImage>();
        return inMemoryImage.Id;
    }
}