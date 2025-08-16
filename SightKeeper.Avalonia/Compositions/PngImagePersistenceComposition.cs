using System.IO;
using Pure.DI;
using SightKeeper.Application;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;
using SixLabors.ImageSharp.Formats.Png;

namespace SightKeeper.Avalonia.Compositions;

internal sealed class PngImagePersistenceComposition
{
	private void Setup() => DI.Setup(nameof(PngImagePersistenceComposition), CompositionKind.Internal)

		.Bind<ImageWrapper>()
		.To<StorableImageWrapper>(_ =>
		{
			FileSystemDataAccess dataAccess = new()
			{
				DirectoryPath = Path.Combine(FileSystemDataAccess.DefaultDirectoryPath, "Images"),
				FileExtension = "png"
			};
			return new StorableImageWrapper(dataAccess);
		})

		.Bind<ImageDataSaver<TTPixel>>()
		.To(_ =>
		{
			var encoder = new PngEncoder();
			return new ImageSharpImageDataSaver<TTPixel>(encoder);
		})
	
		.Bind<ImageLoader<TTPixel>>()
		.To<ImageSharpImageLoader<TTPixel>>();
}