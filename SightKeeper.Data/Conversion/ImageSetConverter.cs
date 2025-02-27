using System.Collections.Immutable;
using SightKeeper.Data.Model;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Conversion;

internal sealed class ImageSetConverter
{
	public ImageSetConverter(FileSystemImageDataAccess imageDataAccess)
	{
		_imageDataAccess = imageDataAccess;
	}

	public IEnumerable<PackableImageSet> ConvertImageSets(IEnumerable<ImageSet> libraries)
	{
		return libraries.Select(ConvertImageSet);
	}

	private readonly FileSystemImageDataAccess _imageDataAccess;

	private PackableImageSet ConvertImageSet(ImageSet library) => new()
	{
		Name = library.Name,
		Description = library.Description,
		Images = ConvertImages(library.Images).ToImmutableArray()
	};

	private IEnumerable<PackableImage> ConvertImages(IEnumerable<Image> images)
	{
		return images.Select(ConvertImage);
	}

	private PackableImage ConvertImage(Image image)
	{
		return new PackableImage
		{
			Id = _imageDataAccess.GetId(image),
			CreationTimestamp = image.CreationTimestamp,
			Image = image.Size
		};
	}
}