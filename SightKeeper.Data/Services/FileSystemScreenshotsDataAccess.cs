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

	public override Stream LoadImage(Image image)
	{
		return new ZLibStream(_fileSystemDataAccess.OpenReadStream(image), CompressionMode.Decompress);
	}

	public Id GetId(Image image)
	{
		return _fileSystemDataAccess.GetId(image);
	}

	public override void DeleteScreenshot(ScreenshotsLibrary library, int index)
	{
		lock (_appDataLock)
			base.DeleteScreenshot(library, index);
		_appDataAccess.SetDataChanged();
	}

	internal void AssociateId(Image image, Id id)
	{
		_fileSystemDataAccess.AssociateId(image, id);
	}

	protected override Image CreateScreenshot(
		ScreenshotsLibrary library,
		DateTimeOffset creationTimestamp,
		Vector2<ushort> resolution)
	{
		Image image;
		lock (_appDataLock)
			image = base.CreateScreenshot(library, creationTimestamp, resolution);
		_appDataAccess.SetDataChanged();
		return image;
	}

	protected override void SaveScreenshotData(Image image, ReadOnlySpan2D<Rgba32> data)
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

	protected override void DeleteScreenshotData(Image image)
	{
		_fileSystemDataAccess.Delete(image);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly Lock _appDataLock;
	private readonly FileSystemDataAccess<Image> _fileSystemDataAccess = new(".bin");
}