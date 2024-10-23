using System.IO.Compression;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;
using FlakeId;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.Binary.Services;

public sealed class FileSystemScreenshotsDataAccess : ScreenshotsDataAccess
{
	public string DirectoryPath
	{
		get => _dataAccess.DirectoryPath;
		set => _dataAccess.DirectoryPath = value;
	}

	public override Stream LoadImage(Screenshot screenshot)
	{
		return new ZLibStream(_dataAccess.OpenReadStream(screenshot), CompressionMode.Decompress);
	}

	public Id GetId(Screenshot screenshot)
	{
		return _dataAccess.GetId(screenshot);
	}

	internal void AssociateId(Screenshot screenshot, Id id)
	{
		_dataAccess.AssociateId(screenshot, id);
	}

	protected override void SaveScreenshotData(Screenshot screenshot, ReadOnlySpan2D<Rgba32> imageData)
	{
		using var stream = new ZLibStream(_dataAccess.OpenWriteStream(screenshot), CompressionLevel.SmallestSize);
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
		_dataAccess.Delete(screenshot);
	}

	private readonly FileSystemDataAccess<Screenshot> _dataAccess = new(".bin");
}