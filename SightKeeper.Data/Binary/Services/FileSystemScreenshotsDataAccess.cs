using System.Collections.Immutable;
using System.IO.Compression;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;
using FlakeId;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemScreenshotsDataAccess : ScreenshotsDataAccess
{
	public string DirectoryPath
	{
		get => _screenshotsDataAccess.DirectoryPath;
		set => _screenshotsDataAccess.DirectoryPath = value;
	}

	public FileSystemScreenshotsDataAccess(AppDataAccess appDataAccess, object locker)
	{
		_appDataAccess = appDataAccess;
		_locker = locker;
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

	internal void DeleteAllScreenshotsData(ScreenshotsLibrary library)
	{
		foreach (var screenshot in library.Screenshots)
			DeleteScreenshotData(screenshot);
	}

	protected override Screenshot CreateScreenshotInLibrary(
		ScreenshotsLibrary library,
		DateTimeOffset creationDate,
		Vector2<ushort> resolution,
		out ImmutableArray<Screenshot> removedScreenshots)
	{
		Screenshot screenshot;
		lock (_locker)
			screenshot = base.CreateScreenshotInLibrary(library, creationDate, resolution, out removedScreenshots);
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

	protected override void DeleteScreenshotFromLibrary(Screenshot screenshot)
	{
		lock (_locker)
			base.DeleteScreenshotFromLibrary(screenshot);
		_appDataAccess.SetDataChanged();
	}

	protected override void DeleteScreenshotData(Screenshot screenshot)
	{
		_screenshotsDataAccess.Delete(screenshot);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly object _locker;
	private readonly FileSystemDataAccess<Screenshot> _screenshotsDataAccess = new(".bin");
}