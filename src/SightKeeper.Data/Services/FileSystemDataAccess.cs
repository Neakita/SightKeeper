using FlakeId;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.Services;

internal class FileSystemDataAccess(string fileExtension)
{
	public const string DefaultDirectoryPath = "Data";

	public string DirectoryPath { get; set; } = DefaultDirectoryPath;
	public string FileExtension => fileExtension;

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
		var fileName = $"{id}.{fileExtension}";
		var filePath = Path.Combine(DirectoryPath, fileName);
		return filePath;
	}
}