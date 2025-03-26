using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using SightKeeper.Avalonia.Annotation.Images;
using SightKeeper.Domain;

namespace SightKeeper.Avalonia;

internal sealed class DesignImageDataContext : ImageDataContext
{
	private const string SamplesDirectoryName = "Samples";

	public DesignImageDataContext(string sampleImageFileName)
	{
		DirectoryInfo workingDirectory = new(Directory.GetCurrentDirectory());
		var projectDirectory = workingDirectory.Parent!.Parent!.Parent;
		_sampleImageFilePath = Path.Combine(projectDirectory!.FullName, SamplesDirectoryName, sampleImageFileName);
	}

	public Task<Bitmap?> Load(int? maximumLargestDimension, CancellationToken cancellationToken)
	{
		Bitmap bitmap = new(_sampleImageFilePath);
		if (maximumLargestDimension == null)
			return Task.FromResult<Bitmap?>(bitmap);
		var targetSize = ImageLoader.ComputeSize(new Vector2<ushort>((ushort)bitmap.PixelSize.Width, (ushort)bitmap.PixelSize.Height), maximumLargestDimension.Value);
		var scaledBitmap = bitmap.CreateScaledBitmap(targetSize, BitmapInterpolationMode.None);
		return Task.FromResult<Bitmap?>(scaledBitmap);
	}

	private readonly string _sampleImageFilePath;
}