using FlakeId;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.Services;

internal class FileSystemDataAccess(ILogger logger)
{
	public const string DefaultDirectoryPath = "Data";

	public string DirectoryPath { get; set; } = DefaultDirectoryPath;
	public required string FileExtension { get; set; }

	public async Task<Image?> LoadImageAsync(Id id, CancellationToken cancellationToken)
	{
		await using var stream = OpenRead(id);
		try
		{
			return await Image.LoadAsync(stream, cancellationToken);
		}
		catch (TaskCanceledException exception)
		{
			logger.Verbose(exception, "Image loading was cancelled");
			return null;
		}
	}

	public async Task<Image<TPixel>?> LoadImageAsync<TPixel>(Id id, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		await using var stream = OpenRead(id);
		try
		{
			return await Image.LoadAsync<TPixel>(stream, cancellationToken);
		}
		catch (TaskCanceledException exception)
		{
			logger.Verbose(exception, "Image loading was cancelled");
			return null;
		}
	}

	public virtual Stream OpenRead(Id id)
	{
		var filePath = GetFilePath(id);
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