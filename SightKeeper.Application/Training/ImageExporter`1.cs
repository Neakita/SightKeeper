using CommunityToolkit.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SightKeeper.Domain.Images.Image;
using MemoryExtensions = CommunityToolkit.HighPerformance.MemoryExtensions;

namespace SightKeeper.Application.Training;

public sealed class ImageExporter<TPixel> : ImageExporter where TPixel : unmanaged, IPixel<TPixel>
{
	public async Task ExportImageAsync(string filePath, Image image, CancellationToken cancellationToken)
	{
		if (TryCopy(filePath, image))
			return;
		if (await TryCopyViaStreamAsync(filePath, image, cancellationToken))
			return;
		if (await TryCopyWithTranscodingFromBinaryAsync(filePath, image, cancellationToken))
			return;
		throw new Exception($"Unable to export image {image} with format {image.DataFormat}");
	}

	private TPixel[] _buffer = Array.Empty<TPixel>();

	private static bool TryCopy(string filePath, Image image)
	{
		var targetExtension = Path.GetExtension(filePath.AsSpan());
		// remove the period
		targetExtension = targetExtension[1..];
		return image.DataFormat == targetExtension && image.TryCopyTo(filePath);
	}

	private static async Task<bool> TryCopyViaStreamAsync(string filePath, Image image, CancellationToken cancellationToken)
	{
		var targetExtension = Path.GetExtension(filePath.AsSpan());
		// remove the period
		targetExtension = targetExtension[1..];
		if (image.DataFormat != targetExtension)
			return false;
		await using var readStream = image.OpenReadStream();
		Guard.IsNotNull(readStream);
		await using var writeStream = File.OpenWrite(filePath);
		await readStream.CopyToAsync(writeStream, cancellationToken);
		return true;
	}

	private async Task<bool> TryCopyWithTranscodingFromBinaryAsync(string filePath, Image image, CancellationToken cancellationToken)
	{
		if (image.DataFormat != "bin")
			return false;
		var requiredBufferLength = image.Size.X * image.Size.Y;
		if (_buffer.Length < requiredBufferLength)
			_buffer = new TPixel[requiredBufferLength];
		await using var imageStream = image.OpenReadStream();
		Guard.IsNotNull(imageStream);
		int bytesRead;
		int totalBytesRead = 0;
		do
		{
			bytesRead = await imageStream.ReadAsync(MemoryExtensions.AsBytes(_buffer.AsMemory())[totalBytesRead..], cancellationToken);
			totalBytesRead += bytesRead;
		} while (bytesRead > 0);
		using var imageSharpImage = SixLabors.ImageSharp.Image.WrapMemory<TPixel>(_buffer, image.Size.X, image.Size.Y);
		await imageSharpImage.SaveAsync(filePath, cancellationToken);
		return true;
	}
}