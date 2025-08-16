using System.IO;
using Pure.DI;
using SightKeeper.Application;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;

namespace SightKeeper.Avalonia.Compositions;

internal sealed class BinaryImagePersistenceComposition
{
	private void Setup() => DI.Setup(nameof(BinaryImagePersistenceComposition), CompositionKind.Internal)

		.Bind<ImageWrapper>()
		.To<StorableImageWrapper>(_ =>
		{
			CompressedFileSystemDataAccess dataAccess = new()
			{
				DirectoryPath = Path.Combine(FileSystemDataAccess.DefaultDirectoryPath, "Images"),
				FileExtension = "bin"
			};
			return new StorableImageWrapper(dataAccess);
		})

		.Bind<ImageDataSaver<TTPixel>>()
		.To<BinaryImageDataSaver<TTPixel>>()
	
		.Bind<ImageLoader<TTPixel>>()
		.To<BinaryImageLoader<TTPixel>>();
}