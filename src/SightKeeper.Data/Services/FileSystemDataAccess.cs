using FlakeId;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.Services;

internal class FileSystemDataAccess
{
	public const string DefaultDirectoryPath = "Data";

	public string DirectoryPath { get; set; } = DefaultDirectoryPath;
	public required string FileExtension { get; set; }

	public Image? LoadImage(Id id, CancellationToken cancellationToken)
	{
		using var stream = OpenRead(id);
		if (stream == null || cancellationToken.IsCancellationRequested)
			return null;
		return Image.Load(stream);
	}

	public Image<TPixel>? LoadImage<TPixel>(Id id, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		using var stream = OpenRead(id);
		if (stream == null)
			return null;
		if (cancellationToken.IsCancellationRequested)
			return null;
		return Image.Load<TPixel>(stream);
	}

	public async Task<Image?> LoadImageAsync(Id id, CancellationToken cancellationToken)
	{
		await using var stream = OpenRead(id);
		if (stream == null)
			return null;
		return await Image.LoadAsync(stream, cancellationToken);
	}

	public async Task<Image<TPixel>?> LoadImageAsync<TPixel>(Id id, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		await using var stream = OpenRead(id);
		if (stream == null)
			return null;
		return await Image.LoadAsync<TPixel>(stream, cancellationToken);
	}

	public virtual Stream? OpenRead(Id id)
	{
		var filePath = GetFilePath(id);
		if (!File.Exists(filePath))
			return null;
		return File.OpenRead(filePath);
	}

	public virtual Stream OpenWrite(Id id)
	{
		Directory.CreateDirectory(DirectoryPath);
		var filePath = GetFilePath(id);
		return File.OpenWrite(filePath);
	}

	public void Delete(Id id)
	{
		var filePath = GetFilePath(id);
		File.Delete(filePath);
	}

	private string GetFilePath(Id id)
	{
		var fileName = $"{id}.{FileExtension}";
		var filePath = Path.Combine(DirectoryPath, fileName);
		return filePath;
	}
}