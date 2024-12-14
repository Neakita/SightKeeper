using System.IO.Compression;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;
using FlakeId;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Domain;
using SightKeeper.Domain.Screenshots;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemScreenshotsDataAccess : ScreenshotsDataAccess
{
	public string DirectoryPath
	{
		get => _screenshotsDataAccess.DirectoryPath;
		set => _screenshotsDataAccess.DirectoryPath = value;
	}

	public FileSystemScreenshotsDataAccess(AppDataAccess appDataAccess, AppDataEditingLock editingLock)
	{
		_appDataAccess = appDataAccess;
		_editingLock = editingLock;
	}

	public override Stream LoadImage(Screenshot screenshot)
	{
		return new ZLibStream(_screenshotsDataAccess.OpenReadStream(screenshot), CompressionMode.Decompress);
	}

	public Id GetId(Screenshot screenshot)
	{
		return _screenshotsDataAccess.GetId(screenshot);
	}

	internal void AssociateId(Screenshot screenshot, Id id)
	{
		_screenshotsDataAccess.AssociateId(screenshot, id);
	}

	protected override Screenshot CreateScreenshot(
		ScreenshotsLibrary library,
		DateTimeOffset creationDate,
		Vector2<ushort> resolution)
	{
		Screenshot screenshot;
		lock (_editingLock)
			screenshot = base.CreateScreenshot(library, creationDate, resolution);
		_appDataAccess.SetDataChanged();
		return screenshot;
	}

	protected override void SaveScreenshotData(Screenshot screenshot, ReadOnlySpan2D<Rgba32> imageData)
	{
		using var stream = new ZLibStream(_screenshotsDataAccess.OpenWriteStream(screenshot), CompressionLevel.SmallestSize);
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
		_screenshotsDataAccess.Delete(screenshot);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly AppDataEditingLock _editingLock;
	private readonly FileSystemDataAccess<Screenshot> _screenshotsDataAccess = new(".bin");
}