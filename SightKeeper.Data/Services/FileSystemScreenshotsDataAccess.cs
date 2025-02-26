using System.IO.Compression;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;
using FlakeId;
using SightKeeper.Application;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.Services;

public sealed class FileSystemScreenshotsDataAccess : ScreenshotsDataAccess
{
	public string DirectoryPath
	{
		get => _fileSystemDataAccess.DirectoryPath;
		set => _fileSystemDataAccess.DirectoryPath = value;
	}

	public FileSystemScreenshotsDataAccess(AppDataAccess appDataAccess, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_appDataAccess = appDataAccess;
		_appDataLock = appDataLock;
	}

	public override Stream LoadImage(Screenshot screenshot)
	{
		return new ZLibStream(_fileSystemDataAccess.OpenReadStream(screenshot), CompressionMode.Decompress);
	}

	public Id GetId(Screenshot screenshot)
	{
		return _fileSystemDataAccess.GetId(screenshot);
	}

	public override void DeleteScreenshot(ScreenshotsLibrary library, int index)
	{
		lock (_appDataLock)
			base.DeleteScreenshot(library, index);
		_appDataAccess.SetDataChanged();
	}

	internal void AssociateId(Screenshot screenshot, Id id)
	{
		_fileSystemDataAccess.AssociateId(screenshot, id);
	}

	protected override Screenshot CreateScreenshot(
		ScreenshotsLibrary library,
		DateTimeOffset creationTimestamp,
		Vector2<ushort> resolution)
	{
		Screenshot screenshot;
		lock (_appDataLock)
			screenshot = base.CreateScreenshot(library, creationTimestamp, resolution);
		_appDataAccess.SetDataChanged();
		return screenshot;
	}

	protected override void SaveScreenshotData(Screenshot screenshot, ReadOnlySpan2D<Rgba32> imageData)
	{
		using var stream = new ZLibStream(_fileSystemDataAccess.OpenWriteStream(screenshot), CompressionLevel.SmallestSize);
		if (imageData.TryGetSpan(out var contiguousData))
		{
			var bytes = MemoryMarshal.AsBytes(contiguousData);
			stream.Write(bytes);
			return;
		}
		for (int i = 0; i < imageData.Height; i++)
		{
			var rowData = imageData.GetRowSpan(i);
			var bytes = MemoryMarshal.AsBytes(rowData);
			stream.Write(bytes);
		}
	}

	protected override void DeleteScreenshotData(Screenshot screenshot)
	{
		_fileSystemDataAccess.Delete(screenshot);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly Lock _appDataLock;
	private readonly FileSystemDataAccess<Screenshot> _fileSystemDataAccess = new(".bin");
}