using CommunityToolkit.Diagnostics;
using FlakeId;
using Serilog;

namespace SightKeeper.Data.Services;

public sealed class FileSystemDataAccess<T> where T : notnull
{
	private readonly ILogger _logger = Log.ForContext<FileSystemDataAccess<T>>();

	public string DirectoryPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "Data");

	public FileSystemDataAccess(string fileExtension = "")
	{
		_fileExtension = fileExtension;
	}

	public Id GetId(T item)
	{
		return _ids[item];
	}

	public FileStream OpenReadStream(T item)
	{
		var filePath = GetFilePath(item);
		return File.OpenRead(filePath);
	}

	public byte[] ReadAllBytes(T item)
	{
		var filePath = GetFilePath(item);
		return File.ReadAllBytes(filePath);
	}

	public void WriteAllBytes(T item, byte[] data)
	{
		var id = Id.Create();
		_ids.Add(item, id);
		var filePath = GetFilePath(id);
		Directory.CreateDirectory(DirectoryPath);
		File.WriteAllBytes(filePath, data);
	}

	public FileStream OpenWriteStream(T item)
	{
		var id = Id.Create();
		_ids.Add(item, id);
		var filePath = GetFilePath(id);
		Directory.CreateDirectory(DirectoryPath);
		return File.OpenWrite(filePath);
	}

	public void AssociateId(T item, Id id)
	{
		Guard.IsTrue(File.Exists(GetFilePath(id)));
		_ids.Add(item, id);
	}

	public void Delete(T item)
	{
		Guard.IsTrue(_ids.Remove(item, out var id));
		var filePath = GetFilePath(id);
		File.Delete(filePath);
	}

	public void ClearUnassociatedFiles()
	{
		if (!Directory.Exists(DirectoryPath))
			return;
		var filesPaths = Directory.GetFiles(DirectoryPath);
		var associatedIds = _ids.Values.ToHashSet();
		foreach (var filePath in filesPaths)
		{
			var fileName = Path.GetFileName(filePath);
			Id fileId = new(long.Parse(fileName.Replace(_fileExtension, "")));
			if (!associatedIds.Contains(fileId))
			{
				File.Delete(filePath);
				_logger.Warning("Unassociated file removed: {filePath}", filePath);
			}
		}
	}

	private readonly Dictionary<T, Id> _ids = new();
	private readonly string _fileExtension;

	private string GetFilePath(T item) => GetFilePath(_ids[item]);
	private string GetFilePath(Id id) => Path.Combine(DirectoryPath, id.ToString()) + _fileExtension;
}