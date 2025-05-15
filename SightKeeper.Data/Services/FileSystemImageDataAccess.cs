using System.IO.Compression;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.Services;

public sealed class FileSystemImageDataAccess : WriteImageDataAccess, ReadImageDataAccess
{
	public FileSystemImageDataAccess(FileSystemDataAccess<Image> fileSystemDataAccess)
	{
		_fileSystemDataAccess = fileSystemDataAccess;
	}

	public Stream LoadImage(Image image)
	{
		return new ZLibStream(_fileSystemDataAccess.OpenReadStream(image), CompressionMode.Decompress);
	}

	public void SaveImageData(Image image, ReadOnlySpan2D<Rgba32> data)
	{
		using var stream = new ZLibStream(_fileSystemDataAccess.OpenWriteStream(image), CompressionLevel.SmallestSize);
		if (data.TryGetSpan(out var contiguousData))
		{
			var bytes = MemoryMarshal.AsBytes(contiguousData);
			stream.Write(bytes);
			return;
		}
		for (int i = 0; i < data.Height; i++)
		{
			var rowData = data.GetRowSpan(i);
			var bytes = MemoryMarshal.AsBytes(rowData);
			stream.Write(bytes);
		}
	}

	public void DeleteImageData(Image image)
	{
		_fileSystemDataAccess.Delete(image);
	}

	private readonly FileSystemDataAccess<Image> _fileSystemDataAccess;
}