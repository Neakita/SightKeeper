using System.Collections.Immutable;
using SightKeeper.Data.Model;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;
using PackableScreenshotsLibrary = SightKeeper.Data.Model.PackableScreenshotsLibrary;

namespace SightKeeper.Data.Conversion;

internal sealed class ScreenshotsLibraryConverter
{
	public ScreenshotsLibraryConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public IEnumerable<PackableScreenshotsLibrary> ConvertScreenshotsLibraries(IEnumerable<ImageSet> libraries)
	{
		return libraries.Select(ConvertScreenshotsLibrary);
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private PackableScreenshotsLibrary ConvertScreenshotsLibrary(ImageSet library) => new()
	{
		Name = library.Name,
		Description = library.Description,
		Screenshots = ConvertScreenshots(library.Screenshots).ToImmutableArray()
	};

	private IEnumerable<PackableScreenshot> ConvertScreenshots(IEnumerable<Image> screenshots)
	{
		return screenshots.Select(ConvertScreenshot);
	}

	private PackableScreenshot ConvertScreenshot(Image image)
	{
		return new PackableScreenshot
		{
			Id = _screenshotsDataAccess.GetId(image),
			CreationTimestamp = image.CreationTimestamp,
			ImageSize = image.ImageSize
		};
	}
}