using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
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

	public Task<Bitmap?> LoadAsync(int? maximumLargestDimension, CancellationToken cancellationToken)
	{
		Bitmap bitmap = new(_sampleImageFilePath);
		if (maximumLargestDimension == null)
			return Task.FromResult<Bitmap?>(bitmap);
		var targetSize = ComputeSize(new Vector2<ushort>((ushort)bitmap.PixelSize.Width, (ushort)bitmap.PixelSize.Height), maximumLargestDimension.Value);
		var scaledBitmap = bitmap.CreateScaledBitmap(targetSize, BitmapInterpolationMode.None);
		return Task.FromResult<Bitmap?>(scaledBitmap);
	}

	private static PixelSize ComputeSize(Vector2<ushort> imageSize, int maximumLargestDimension)
	{
		var sourceLargestDimension = Math.Max(imageSize.X, imageSize.Y);
		if (sourceLargestDimension < maximumLargestDimension)
			return new PixelSize(imageSize.X, imageSize.Y);
		var size = imageSize.ToInt32() * maximumLargestDimension / sourceLargestDimension;
		return new PixelSize(size.X, size.Y);
	}

	private readonly string _sampleImageFilePath;
}